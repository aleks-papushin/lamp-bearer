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

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = safe;

            _lighting = GetComponentInChildren<WallLighting>(true);
            _animation = GetComponent<WallAnimation>();

            IsDangerous = false;        
        }

        public IEnumerator BecameDangerousCoroutine(float _wallWarningInterval)
        {
            //_spriteRenderer.sprite = warning;
            _animation.MakeWarning();
            _lighting.SetWarning();

            yield return new WaitForSeconds(_wallWarningInterval);

            //_spriteRenderer.sprite = danger;
            _animation.BecameDanger();
            _lighting.SetDanger();
            IsDangerous = true;
        }

        public void BecameSafe()
        {
            //_spriteRenderer.sprite = safe;
            _animation.BecameSafe();
            _lighting.Disable();
            IsDangerous = false;
        }
    }
}
