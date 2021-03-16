using UnityEngine;

public abstract class GravityHandler : MonoBehaviour
{
    [SerializeField] protected float _gravity = 9.8f;

    public Direction GravityVector { get; protected set; }

    public abstract void SwitchLocalGravity(Direction direction);
}