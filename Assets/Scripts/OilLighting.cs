using Assets.Scripts.Resources;
using UnityEngine;

public class OilLighting : MonoBehaviour
{
    private GameObject _player;
    private Light _light;
    [SerializeField] private float _maxLightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(TagNames.PlayerTag);
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        this.HandleLighting();
    }

    private void HandleLighting()
    {
        var distance = Vector2.Distance(transform.position, _player.transform.position);
        _light.intensity = _maxLightIntensity / (distance * (distance * 0.5f) + 1);
    }
}
