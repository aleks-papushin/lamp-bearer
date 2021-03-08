using Assets.Scripts.Resources;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerWallCollisions : ObjectWallCollisions
    {
        public static event Action<bool> OnIsGroundedChanged;

        private void OnCollisionEnter2D(Collision2D otherCollider)
        {
            if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
            HandleCollisionState(otherCollider, 1);
            OnIsGroundedChanged?.Invoke(IsGrounded);
        }

        private void OnCollisionExit2D(Collision2D otherCollider)
        {
            if (!otherCollider.gameObject.tag.Contains(Tags.WallSuffix)) return;
            HandleCollisionState(otherCollider, -1);
            OnIsGroundedChanged?.Invoke(IsGrounded);
        }
    }
}
