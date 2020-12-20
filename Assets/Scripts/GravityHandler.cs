using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class GravityHandler : MonoBehaviour, IGravitySwitcher
    {
        [SerializeField] protected float _gravity = 9.8f;

        public Direction GravityVector { get; set; }

        public abstract void SwitchGravity(Direction direction);
    }
}
