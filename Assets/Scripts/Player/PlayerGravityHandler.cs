using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerGravityHandler : MonoBehaviour, IGravitySwitcher
    {
        private readonly float _gravity = 9.8f;

        public Direction GravityVector { get; set; }

        public bool IsGravityVectorVertical => GravityVector == Direction.Down || GravityVector == Direction.Up;
        public bool IsGravityVectorHorizontal => GravityVector == Direction.Left || GravityVector == Direction.Right;

        public void SwitchGravity(Direction direction)
        {
            GravityVector = direction;

            switch (GravityVector)
            {
                case Direction.Down:
                default:
                    Physics2D.gravity = new Vector2(0, -_gravity);
                    break;
                case Direction.Up:
                    Physics2D.gravity = new Vector2(0, _gravity);
                    break;
                case Direction.Left:
                    Physics2D.gravity = new Vector2(-_gravity, 0);
                    break;
                case Direction.Right:
                    Physics2D.gravity = new Vector2(_gravity, 0);
                    break;
            }
        }
    }
}
