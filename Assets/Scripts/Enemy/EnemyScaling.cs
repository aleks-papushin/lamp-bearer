using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyScaling : MonoBehaviour
    {
        [SerializeField] private float _initScaleMultiplier;
        [SerializeField] private float _incrementValue;
        private float _defaultScaleX;
        private EnemyWalkerMovement _movement;

        public bool IsIncrease { get; set; }
        public bool IsDestroy { get; set; }

        private void Start()
        {
            var localScale = transform.localScale;
            _defaultScaleX = Math.Abs(localScale.x);
            localScale *= _initScaleMultiplier;
            transform.localScale = localScale;
            _movement = GetComponent<EnemyWalkerMovement>();
        }

        private void Update()
        {
            if (IsIncrease)
            {
                Increase();
            }
            else if (IsDestroy)
            {
                DecreaseAndDestroy();
            }
        }

        private Vector3 GetScaleIncrementVector()
        {
            var localScale = transform.localScale;
            var x = localScale.x > 0 ? _incrementValue : -_incrementValue;
            var y = localScale.y > 0 ? _incrementValue : -_incrementValue;
            var z = localScale.z > 0 ? _incrementValue : -_incrementValue;

            return new Vector3(x, y, z);
        }

        private void DecreaseAndDestroy()
        {
            var finalScale = _initScaleMultiplier * 0.1;
            if (Math.Abs(transform.localScale.x) > finalScale)
            {
                transform.localScale -= GetScaleIncrementVector() * (_movement.Speed * Time.deltaTime * 25);            
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Increase()
        {
            if (Math.Abs(transform.localScale.x) < _defaultScaleX)
            {
                transform.localScale += GetScaleIncrementVector() * (_movement.Speed * Time.deltaTime * 25);
            }
            else
            {
                IsIncrease = false;
            }
        }
    }
}
