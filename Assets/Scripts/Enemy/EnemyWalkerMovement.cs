﻿using Resources;
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
        
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var directionMod = IsDirectionPositive ? 1 : -1;

            _facing.Handle(IsDirectionPositive);
            switch (Wall.tag)
            {
                case Tags.BottomWall:
                    _rig.velocity = new Vector2(directionMod, 0) * _speed;
                    break;
                case Tags.LeftWall:
                    _rig.velocity = new Vector2(0, -directionMod) * _speed;
                    break;
                case Tags.UpperWall:
                    _rig.velocity = new Vector2(-directionMod, 0) * _speed;
                    break;
                case Tags.RightWall:
                    _rig.velocity = new Vector2(0, directionMod) * _speed;
                    break;
            }
        }
    }
}
