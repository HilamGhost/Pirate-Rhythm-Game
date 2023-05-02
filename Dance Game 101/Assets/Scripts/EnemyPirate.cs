using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hilam
{
    public class EnemyPirate : MonoBehaviour
    {
        private Animator enemyAnimator;
        [SerializeField] private float beatSpeed;

        private AudioSource audioSource;

        [Header("Audios")] 
        [SerializeField] private AudioClip _swordSwingSFX;
        void Start()
        {
            enemyAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            SetAnimation();
        }

        void SetAnimation()
        {
            enemyAnimator.SetFloat("Beat Speed", beatSpeed);
        }

        public void PlaySlashAnimation(Vector2 _direction, BeatTypes _beatType)
        {
            float _dirX = _direction.x;
            float _dirY = _direction.y;
            string _triggerName = "";
            
            if (Mathf.Approximately(_dirX, 0))
            {
                if (_dirY > 0) _triggerName = "Up Attack";
                if (_dirY < 0) _triggerName = "Down Attack";
            }
            else
            {
                if (_dirX > 0) _triggerName = "Right Attack";
                if (_dirX < 0) _triggerName = "Left Attack";
            }

            beatSpeed = _beatType switch
            {
                BeatTypes.Eighth => 0.5f,
                BeatTypes.Quarter => 1,
                BeatTypes.Half => 2,
                BeatTypes.Whole => 4
            };
            
            enemyAnimator.SetTrigger(_triggerName);
            
            audioSource.clip = _swordSwingSFX;
            audioSource.PlayDelayed(beatSpeed/4);
            
            Invoke("PlayPlayerHurtAnimation",beatSpeed/2);
        }
        
        public void PlayPlayerHurtAnimation()
        {
            
            RythmManager.Instance.PlayHurt();
        }
    }
}
