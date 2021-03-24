using UnityEngine;

namespace Player
{
    public class PlayerGravityHandler : MonoBehaviour
    {
        public Direction GravityVector { get; private set; }
        
        public void SwitchLocalGravity(Direction direction)
        {
            GravityVector = direction;
        }
    }
}
