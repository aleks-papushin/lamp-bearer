using UnityEngine;

namespace Assets.Scripts.Player
{
    public abstract class PlayerAction
    {
        protected Rigidbody2D _rig;
        protected PlayerCollisions _collisions;

        public PlayerAction(Rigidbody2D rigidbody, PlayerCollisions collisions)
        {
            _rig = rigidbody;
            _collisions = collisions;
        }
    }
}
