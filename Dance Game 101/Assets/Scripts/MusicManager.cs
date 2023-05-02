using System.Collections.Generic;
using UnityEngine;

namespace Hilam
{
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private List<RythmPoint> rhythmPoints;
        [SerializeField] private List<RiffManager> riffs;
        [SerializeField] private float musicDuration;
        [SerializeField] private float musicLenght;
        [SerializeField] private int currentRiff;
        [SerializeField] private int currentPoint;
        private bool isStopped;
        private bool isWinned;

        private AudioSource bgMusic;
        public RythmTimeHolder rth;
        private void Start()
        {
            bgMusic = GetComponent<AudioSource>();
            musicLenght = bgMusic.clip.length;
            Invoke("WinGame",musicLenght);
        }

        private void Update()
        {
            musicDuration = bgMusic.time;
            CheckBeats();
            CheckRiffs();
            WinGame();
            
            if (Input.GetKeyDown(KeyCode.H))
            {
                rth.AddTime(musicDuration);
            }
        }

        public void WinGame()
        {
            if(isStopped || isWinned) return;
            
            if (!bgMusic.isPlaying)
            {
                isWinned = true;
                StartCoroutine(GameManager.Instance.WinGame());
            }
        }
        public void StopMusic()
        {
            bgMusic.Stop();
            isStopped = true;
        }
        void CheckBeats()
        {
           if(isStopped || isWinned) return;

           float _nextBeat = riffs[currentRiff].rhythmPoints[currentPoint].Duration+riffs[currentRiff].time;
           
            if (musicDuration >= _nextBeat && musicDuration <= _nextBeat+0.1f)
            {
                RythmManager.Instance.CreateBeat(riffs[currentRiff].rhythmPoints[currentPoint].BeatType);
                currentPoint++;
            }
        }

        void CheckRiffs()
        {
            if(isStopped || isWinned) return;
            if(musicDuration <= 1) return;
            if(currentRiff+1 > riffs.Count) return;
            
            if (currentPoint >= riffs[currentRiff].rhythmPoints.Count)
            {
                currentRiff++;
                currentPoint = 0;
            }
        }
    }
}
