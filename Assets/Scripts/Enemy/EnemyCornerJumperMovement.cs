using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyCornerJumperMovement : EnemyMovementBase
    {        
        [SerializeField] private JumpOverCorner _cornerJump;

        private void Update()
        {
            MoveWithCornerHandling();
        }

        protected void MoveWithCornerHandling()
        {
            if (_cornerJump.IsCornerReached) return;

            Move();
        }
    }
}
