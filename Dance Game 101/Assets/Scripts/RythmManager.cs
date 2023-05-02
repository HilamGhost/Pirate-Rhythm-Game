using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Hilam
{
    public class RythmManager : Singleton<RythmManager>
    {
        [Header("General Informations")] 
        [SerializeField] private bool isPlaying = true;
        [SerializeField] private bool beatIsPlaying;
        [SerializeField] private bool isBeatPlayed;
        [SerializeField] private Vector2 lastKeyPushed;

        [Space(10)]
        [SerializeField] private int BPM;
        [SerializeField] float beatTime;
        [SerializeField] float beatTimeNeeded;
        [SerializeField] private BeatStatus lastBeatStatus;
        private Beat beat;

        [Space(10)] 
        [Header("UI")] 
        [SerializeField] private Slider beatUI;
        [SerializeField] private GameObject[] beatIndicators = new GameObject[4];
        [SerializeField] private TextMeshProUGUI beatTypeUI;
        [SerializeField] private Image keyShowerImage;
        [SerializeField] private Sprite[] keySprites;
        [SerializeField] private Sprite emptyKeySprite;

        [Space(15)] 
        [Header("Animations")] 
        [SerializeField] private EnemyPirate enemyPirate;
        [SerializeField] private PlayerPirate playerPirate;
        [SerializeField] private bool isHurt;
        public float TimeScale => BPM / 60;
        private IEnumerator Start()
        {
            while (isPlaying)
            {
                yield break;
            }
            
        }

        private void Update()
        {
            CheckTime();
            ChangeUI();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CreateBeat());
            }
            if (IsKeyPushed())
            {
                PlayBeat(lastKeyPushed,beatTime);
                
            }
        }

        bool IsKeyPushed()
        {
            if (isBeatPlayed) return false;
            
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                lastKeyPushed = new Vector2(1,0);
                isBeatPlayed = true;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                lastKeyPushed = new Vector2(0,1);
                isBeatPlayed = true;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                lastKeyPushed = new Vector2(0,-1);
                isBeatPlayed = true;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lastKeyPushed = new Vector2(-1,0);
                isBeatPlayed = true;
                return true;
            }

            return false;
        }
        IEnumerator CreateBeat()
        {
            Debug.Log("Created Beat");
            yield return new WaitForSeconds(1);
            Debug.Log("Now!");
            CreateBeatRandom();
            Debug.Log($"Beat is {beat.BeatType}");

        }
        void CheckTime()
        {
            if(beat == null) return;
            
            beatTime += Time.deltaTime*TimeScale;
            
            if (beatTime >= beatTimeNeeded)
            { 
                MissBeat(); //missed
            }
            
        }

        void PlayBeat(Vector2 _direction, float _time)
        {
            if(beat == null) return;
            
            beat.PlayBeat(_direction,beatTime);
            
            
            DestroyBeat();
            AddPoints();
            
        }

        void MissBeat()
        {
            if(beat == null) return;
            
            isHurt = true;
            beat.MissBeat();
            DestroyBeat();
            
            AddPoints();
            isHurt = false;
        }
        
        void CreateBeatRandom()
        {
            int _randomNumber = Random.Range(0,4);
            BeatTypes _beatType = _randomNumber switch {0=> BeatTypes.Quarter,1=> BeatTypes.Eighth,2=>BeatTypes.Half,3 => BeatTypes.Whole };
            
            beat = new Beat(RandomDirection(), _beatType);
            beatIsPlaying = true;
            beatTimeNeeded = beat.HitTime;
            
            isBeatPlayed = false;
            enemyPirate.PlaySlashAnimation(beat.Direction,beat.BeatType);
            

        }
        public void CreateBeat(BeatTypes _beatTypeWanted)
        {

            BeatTypes _beatType = _beatTypeWanted;
            
            beat = new Beat(RandomDirection(), _beatType);
            beatIsPlaying = true;
            beatTimeNeeded = beat.HitTime;
            
            isBeatPlayed = false;
            enemyPirate.PlaySlashAnimation(beat.Direction,beat.BeatType);
            

        }
        Vector2 RandomDirection()
        {
            int _dirX = Random.Range(-1, 2);
            int _dirY = 0;
            if (_dirX == 0)
            {
                bool isNotZero = false;
                while (!isNotZero)
                {
                    _dirY = Random.Range(-1, 2);
                    if (_dirY != 0) isNotZero = true;
                }
            }

            return new Vector2(_dirX, _dirY);
        }
        void AddPoints()
        {
            int _value = 0;
             _value = lastBeatStatus switch
            {
                BeatStatus.Late => 5,
                BeatStatus.Early=> 5,
                BeatStatus.Perfect => 10,
                BeatStatus.Missed => -10
            };

             if (lastBeatStatus == BeatStatus.Missed)
             {
                 GameManager.Instance.RemoveHealth();
                 isHurt = true;
             }
             else
             {
                 playerPirate.PlayDeflectAnimation();
             }
                
             
            GameManager.Instance.AddPoints(_value);
        }

        public void PlayHurt()
        {
            if (!isHurt) return;
            
            playerPirate.PlayHurtAnimation();
            isHurt = false;
        }
        
        void ChangeUI()
        {
            beatUI.value = beatTime;
            beatUI.maxValue = beatTimeNeeded;
            beatTypeUI.enabled = !beatIsPlaying;
            beatTypeUI.text = $"{lastBeatStatus}!";
            
            #region Indicators
            int _numberValue = -1;
                
            if (beat != null)
            {
                _numberValue = beat.BeatType switch
                { 
                    BeatTypes.Eighth => 0,
                    BeatTypes.Half => 1,
                    BeatTypes.Quarter => 2,
                    BeatTypes.Whole => 3
                };
            }
            
            if(_numberValue > -1) beatIndicators[_numberValue].SetActive(true);
                
            #endregion
            #region Key Directions

            if (beat != null)
            {
                int _direction = beat.BeatDirection() switch
                {
                    "Up" =>0,
                    "Right" =>1,
                    "Down" =>2,
                    "Left" =>3,
                };
                keyShowerImage.sprite = keySprites[_direction];
            }
            else
            {
                keyShowerImage.sprite = emptyKeySprite;
            }
           

            #endregion

        }
        void DestroyBeat()
        {
            lastBeatStatus = beat.BeatStatus;
            Debug.Log(lastBeatStatus);

            beatTime = 0;
            beat = null;
            
            beatIsPlaying = false;
            
            foreach (var _indicators in beatIndicators)
            {
                _indicators.SetActive(false);
            }
            
            
            Debug.ClearDeveloperConsole();
        }
    }
}
