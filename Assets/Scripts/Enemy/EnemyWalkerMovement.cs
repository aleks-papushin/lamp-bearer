using System;
using Resources;
using UnityEngine;

namespace Enemy
{
    public class EnemyWalkerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private HandleObjectFacing _facing;
        private Rigidbody2D _rig;
        public GameObject Wall { get; set; }

        public bool IsDirectionPositive { get; set; } = true;
        
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _facing = GetComponent<HandleObjectFacing>();
        }

        private void FixedUpdate()
        {
            var gravity = Physics.gravity.magnitude;
            switch (Wall.tag)
            {
                case Tags.BottomWall:
                    _rig.AddForce(new Vector2(0, -gravity), ForceMode2D.Force);
                    break;
                case Tags.LeftWall:
                    _rig.AddForce(new Vector2(-gravity, 0), ForceMode2D.Force);
                    break;
                case Tags.UpperWall:
                    _rig.AddForce(new Vector2(0, gravity), ForceMode2D.Force);
                    break;
                case Tags.RightWall:
                    _rig.AddForce(new Vector2(gravity, 0), ForceMode2D.Force);
                    break;
            }
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var directionMod = IsDirectionPositive ? 1 : -1;

            _facing.Handle(IsDirectionPositive);
            _rig.velocity = Wall.tag switch
            {
                Tags.BottomWall => new Vector2(directionMod, 0) * _speed,
                Tags.LeftWall => new Vector2(0, -directionMod) * _speed,
                Tags.UpperWall => new Vector2(-directionMod, 0) * _speed,
                Tags.RightWall => new Vector2(0, directionMod) * _speed,
                _ => _rig.velocity
            };
        }
    }
}