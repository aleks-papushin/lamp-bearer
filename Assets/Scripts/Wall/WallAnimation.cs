using UnityEngine;

public class WallAnimation : MonoBehaviour
{
    private Animator _animator;
    private static readonly int State = Animator.StringToHash("State");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void BecameSafe() => _animator.SetInteger(State, 0);

    public void MakeWarning() => _animator.SetInteger(State, -1);

    public void BecameDanger() => _animator.SetInteger(State, 1);
}
