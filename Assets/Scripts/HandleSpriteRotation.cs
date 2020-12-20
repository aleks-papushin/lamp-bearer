using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class HandleSpriteRotation : MonoBehaviour
    {
        [SerializeField] private float _startRotationDistance;
        [SerializeField] private float _defaultRotationSpeed;
        [SerializeField] private float _rotationSpeedMod;
        [SerializeField] private float _rotationSpeed;
        private readonly float _resetRotationSpeedDistance = 1.5f;

        private GravityHandler _gravityHandler;

        public GameObject Ground => DirectionUtils.GetFloorFor(_gravityHandler.GravityVector);

        // Start is called before the first frame update
        void Awake()
        {
            _rotationSpeed = _defaultRotationSpeed;
            _gravityHandler = GetComponent<GravityHandler>();
        }

        public void Handle()
        {
            GameObject ground = DirectionUtils.GetFloorFor(_gravityHandler.GravityVector);
            var distanceToGround = transform.GetDistanceTo(ground);
            if (distanceToGround < _startRotationDistance)
            {
                this.RotateTowards(_gravityHandler.GravityVector, accelerateRotation: true);
            }
            else
            {
                _rotationSpeed = _defaultRotationSpeed;
                this.RotateTowards(DirectionUtils.OppositeTo(_gravityHandler.GravityVector), accelerateRotation: false);
            }
        }

        private void RotateTowards(Direction gravityVector, bool accelerateRotation)
        {
            if (accelerateRotation)
            {
                // 4 is magic number which is should be bigger than any GetDistanceTo(Ground)
                var modifier = 4 / transform.GetDistanceTo(Ground);
                _rotationSpeed *= modifier;
            }

            var floor = DirectionUtils.GetFloorFor(gravityVector);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, floor.transform.rotation, Time.deltaTime * _rotationSpeed);
        }
    }
}

