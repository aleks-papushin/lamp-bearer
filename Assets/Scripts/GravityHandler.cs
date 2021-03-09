using Enums;
using Interfaces;
using UnityEngine;

public abstract class GravityHandler : MonoBehaviour, IGravitySwitcher
{
    [SerializeField] protected float _gravity = 9.8f;

    public Direction GravityVector { get; protected set; }

    public abstract void SwitchLocalGravity(Direction direction);
}