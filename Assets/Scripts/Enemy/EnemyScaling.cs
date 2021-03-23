using System;
using Resources;
using UnityEngine;

namespace Enemy
{
    public class EnemyScaling : MonoBehaviour
    {
        [SerializeField] private float _initScaleMultiplier;
        [SerializeField] private float _incrementValue;
        private float _defaultScaleX;
        private EnemyMovement _movement;
        private bool _isCreated;
        private bool _isIncreasing;
        private bool _isDestroying;

        private void Start()
        {
            var localScale = transform.localScale;
            _defaultScaleX = Math.Abs(localScale.x);
            transform.localScale = localScale * _initScaleMultiplier;
            _movement = GetComponent<EnemyMovement>();
        }

        private void Update()
        {
            if (_isIncreasing)
            {
                Increase();
            }
            else if (_isDestroying)
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
                _isIncreasing = false;
            }
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.tag.Contains(Tags.SpawnerSuffix)) return;
            if (!_isCreated)
            {
                _isIncreasing = true;
                _isCreated = true;
            }
            else if (!_isIncreasing)
            {
                _isDestroying = true;
            }
            
        }
    }
}
