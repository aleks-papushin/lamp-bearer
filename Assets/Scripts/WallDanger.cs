using System.Collections;
using UnityEngine;

public class WallDanger : MonoBehaviour
{
    public bool IsDangerous { get; private set; }
    public bool IsPlayerStandsOnMe { get; set; }

    public Sprite safe;
    public Sprite warning;
    public Sprite danger;

    private SpriteRenderer _spriteRenderer;
    private WallLighting _lighting;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = safe;

        _lighting = GetComponentInChildren<WallLighting>(true);
        
        IsDangerous = false;        
    }

    public IEnumerator BecameDangerousCoroutine(float _wallWarningInterval)
    {
        _spriteRenderer.sprite = warning;
        _lighting.SetWarning();

        yield return new WaitForSeconds(_wallWarningInterval);

        _spriteRenderer.sprite = danger;
        _lighting.SetDanger();
        IsDangerous = true;
    }

    public void BecameSafe()
    {
        _spriteRenderer.sprite = safe;
        _lighting.Disable();
        IsDangerous = false;
    }
}
