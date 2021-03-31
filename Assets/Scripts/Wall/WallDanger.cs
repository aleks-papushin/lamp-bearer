using System;
using System.Collections;
using UnityEngine;

namespace Wall
{
    public class WallDanger : MonoBehaviour
    {
        [SerializeField] private AudioClip _dangerSound;
        [SerializeField] [Range(0, 1)] private float _volume = 0.2f;
        private WallLighting _lighting;
        private WallAnimation _animation;
        private const float WaitingDangerAnimationEndingInterval = 0.4f;

        public bool IsDangerous { get; private set; }
        public bool CanBeDangerous { get; private set; } = true;

        private void Start()
        {
            _lighting = GetComponentInChildren<WallLighting>(true);
            _animation = GetComponent<WallAnimation>();
        }

        public IEnumerator BecameDangerousCoroutine(
            Action decrementDangerWallsCounter, 
            float wallWarningInterval, 
            float wallDangerousInterval, 
            float wallCoolDownInterval)
        {
            CanBeDangerous = false;

            _animation.MakeWarning();
            _lighting.SetWarning();

            yield return new WaitForSeconds(wallWarningInterval);

            _animation.BecameDanger();
            AudioSource.PlayClipAtPoint(_dangerSound, Camera.main.transform.position, _volume);

            yield return new WaitForSeconds(WaitingDangerAnimationEndingInterval);

            _lighting.SetDanger();
            IsDangerous = true;

            yield return new WaitForSeconds(wallDangerousInterval);

            BecameSafe();
            decrementDangerWallsCounter.Invoke();

            yield return new WaitForSeconds(wallCoolDownInterval);

            CanBeDangerous = true;
        }

        private void BecameSafe()
        {
            _animation.BecameSafe();
            _lighting.Disable();
            IsDangerous = false;            
        }
    }
}
