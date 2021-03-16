using System.Collections;
using UnityEngine;

namespace Wall
{
    public class WallDanger : MonoBehaviour
    {
        public bool IsDangerous { get; private set; }

        public Sprite safe;

        private SpriteRenderer _spriteRenderer;
        private WallLighting _lighting;
        private WallAnimation _animation;
        private const float WaitingDangerAnimationEndingInterval = 0.4f;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = safe;

            _lighting = GetComponentInChildren<WallLighting>(true);
            _animation = GetComponent<WallAnimation>();

            IsDangerous = false;        
        }

        public IEnumerator BecameDangerousCoroutine(float wallWarningInterval)
        {
            _animation.MakeWarning();
            _lighting.SetWarning();

            yield return new WaitForSeconds(wallWarningInterval);

            _animation.BecameDanger();

            yield return new WaitForSeconds(WaitingDangerAnimationEndingInterval);

            _lighting.SetDanger();
            IsDangerous = true;
        }

        public void BecameSafe()
        {
            _animation.BecameSafe();
            _lighting.Disable();
            IsDangerous = false;
        }
    }
}
