using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemFlotante : MonoBehaviour
{
    [SerializeField] private float _amplitud = 0.15f;
    [SerializeField] private float _velocidad = 2f;
    [SerializeField] private float _pulsoLuz = 0.3f;

    private Light2D _luz;
    private float _yInicial;
    private float _intensidadBase;

    private void Awake()
    {
        _yInicial = transform.position.y;
        _luz = GetComponentInChildren<Light2D>();
        if (_luz != null) _intensidadBase = _luz.intensity;
    }

    private void Update()
    {
        float onda = Mathf.Sin(Time.time * _velocidad);

        Vector3 p = transform.position;
        p.y = _yInicial + onda * _amplitud;
        transform.position = p;

        if (_luz != null)
            _luz.intensity = _intensidadBase + onda * _pulsoLuz;
    }
}