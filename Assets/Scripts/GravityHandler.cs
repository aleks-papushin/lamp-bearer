using UnityEngine;

public abstract class GravityHandler : MonoBehaviour
{
    protected readonly float _gravity = Physics.gravity.magnitude;

    public Direction GravityVector { get; protected set; }

    public abstract void SwitchLocalGravity(Direction direction);
}