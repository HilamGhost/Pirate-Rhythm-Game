using UnityEngine;

namespace Hilam
{
    public class PlayerPirate : MonoBehaviour
    {
        private Animator enemyAnimator;
        private AudioSource audioSource;
        
        [Header("Audios")] 
        [SerializeField] private AudioClip _swordDeflectSFX;
        [SerializeField] private AudioClip _hitSFX;
        [SerializeField] private AudioClip _slashSFX;
        
        [Header("VFX")] 
        [SerializeField] private ParticleSystem _playerBloodVFX;
        [SerializeField] private ParticleSystem _playerDeflectVFX;
        void Start()
        {
            enemyAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayDeflectAnimation()
        {
            enemyAnimator.SetTrigger("Deflect");
            audioSource.PlayOneShot(_swordDeflectSFX);
            _playerDeflectVFX.Play();
        }
        public void PlayHurtAnimation()
        {
            enemyAnimator.SetTrigger("Hurt");
            audioSource.PlayOneShot(_hitSFX);
            _playerBloodVFX.Play();
        }

        public void PlayAttackAnimation()
        {
            enemyAnimator.SetTrigger("Attack");
            audioSource.PlayOneShot(_hitSFX);
            audioSource.PlayOneShot(_slashSFX);
        }
    }
}
