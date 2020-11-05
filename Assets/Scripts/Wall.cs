using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private Color _myColor;

    public bool IsDangerous { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _myColor = Color.green;
        this.ChangeColorTo(_myColor);
        
        IsDangerous = false;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BecameDangerous()
    {
        _myColor = Color.red;
        this.ChangeColorTo(_myColor);
        IsDangerous = true;
    }

    public void BecameSafe()
    {
        _myColor = Color.green;
        this.ChangeColorTo(_myColor);
        IsDangerous = false;
    }

    private void ChangeColorTo(Color color)
    {
        transform.GetComponent<SpriteRenderer>().color = color;
    }
}
