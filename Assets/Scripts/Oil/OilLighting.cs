using Assets.Scripts.Resources;
using UnityEngine;

public class OilLighting : MonoBehaviour
{
    private GameObject _player;
    private Light _light;
    [SerializeField] private float _maxLightIntensity;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player);
        _light = GetComponent<Light>();
    }

    void Update()
    {
        if (_player != null)
        {
            this.HandleLighting();
        }        
    }

    private void HandleLighting()
    {
        var distance = Vector2.Distance(transform.position, _player.transform.position);
        _light.intensity = _maxLightIntensity / (distance * (distance * 0.1f) + 1);
    }
}
