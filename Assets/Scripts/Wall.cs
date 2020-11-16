using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public bool IsDangerous { get; private set; }
    public bool IsPlayerStandingOn { get; set; }

    public Sprite safe;
    public Sprite warning;
    public Sprite danger;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //this.ChangeColorTo(Color.green);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = safe;
        
        IsDangerous = false;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BecameDangerous()
    {
        //this.ChangeColorTo(Color.red);
        _spriteRenderer.sprite = danger;
        IsDangerous = true;
    }

    public IEnumerator BecameDangerousCoroutine(float _wallWarningInterval)
    {
        //this.ChangeColorTo(new Color(255/255f, 128/255f, 0/255f));
        _spriteRenderer.sprite = warning;

        yield return new WaitForSeconds(_wallWarningInterval);

        //this.ChangeColorTo(Color.red);
        _spriteRenderer.sprite = danger;
        IsDangerous = true;
    }

    public void BecameSafe()
    {
        //this.ChangeColorTo(Color.green);
        _spriteRenderer.sprite = safe;
        IsDangerous = false;
    }

    private void ChangeColorTo(Color color)
    {
        transform.GetComponent<SpriteRenderer>().color = color;
    }
}
