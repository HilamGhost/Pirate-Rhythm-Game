using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hilam
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private int Points;
        [SerializeField] private int Health= 3;
        [SerializeField] private int Combo;
        [SerializeField] private int ComboNeededForHealth = 10;
        [Space(15)] 
        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI pointUI;

        [SerializeField] private Slider healthBar;

        [Space(15)] 
        [Header("Ragdolls")] 
        [SerializeField] private GameObject playerRagdoll;
        [SerializeField] private GameObject enemyRagdoll;
        
        [Space(15)] 
        [Header("Player and Enemy")] 
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject enemy;

        private void Update()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            pointUI.text = Points.ToString();
            healthBar.value = Health;
        }

        public void AddPoints(int _points)
        {
            Points += _points;
           
            if (Points <= 0) Points = 0;
            
            if (_points > 0) Combo++;
            else Combo = 0;

            if ((Combo>1) && Combo % ComboNeededForHealth == 0)
            {
                AddHealth();
            }
        }

        public void RemoveHealth()
        {
            Health--;
            if (Health <= 0) StartCoroutine(LoseGame());
        }

        IEnumerator LoseGame()
        {
            MusicManager.Instance.StopMusic();
            CreatePlayerRagdoll(player.transform);
            yield return new WaitForSeconds(5);
            Debug.Break();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public IEnumerator WinGame()
        {
            MusicManager.Instance.StopMusic();
            CreateEnemyRagdoll(enemy.transform);
            player.GetComponent<PlayerPirate>().PlayAttackAnimation();
            yield return new WaitForSeconds(5);
            Debug.Break();
        }
        public void AddHealth()
        {
            if(Health >= 3) return;
            Health++;
        }

        public void CreatePlayerRagdoll(Transform _player)
        {
            GameObject _ragdoll = Instantiate(playerRagdoll, _player.position, _player.localRotation);
            Destroy(_player.gameObject);
            
        }
        public void CreateEnemyRagdoll(Transform enemy)
        {
            GameObject _ragdoll = Instantiate(enemyRagdoll, enemy.position, enemy.localRotation);
            Destroy(enemy.gameObject);
            
        }
    }
}
