using System;
using System.Collections;
using UnityEngine;

namespace Wall
{
    public class WallDanger : MonoBehaviour
    {
        public bool IsDangerous { get; private set; }
        public bool CanBeDangerous { get; set; } = true;

        private WallLighting _lighting;
        private WallAnimation _animation;
        private const float WaitingDangerAnimationEndingInterval = 0.4f;

        private void Start()
        {
            _lighting = GetComponentInChildren<WallLighting>(true);
            _animation = GetComponent<WallAnimation>();
        }

        public IEnumerator BecameDangerousCoroutine(
            Action decrementDangerWallsCounter, 
            float wallWarningInterval, 
            float _wallDangerousInterval, 
            float _wallCoolDownInterval)
        {
            CanBeDangerous = false;

            _animation.MakeWarning();
            _lighting.SetWarning();

            yield return new WaitForSeconds(wallWarningInterval);

            _animation.BecameDanger();

            yield return new WaitForSeconds(WaitingDangerAnimationEndingInterval);

            _lighting.SetDanger();
            IsDangerous = true;

            yield return new WaitForSeconds(_wallDangerousInterval);

            BecameSafe();
            decrementDangerWallsCounter.Invoke();

            yield return new WaitForSeconds(_wallCoolDownInterval);

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
