using UnityEngine;

namespace Assets.Scripts.Player
{
    public abstract class PlayerAction : MonoBehaviour
    {
        protected Rigidbody2D _rig;
        protected PlayerCollisions _collisions;
    }
}
