using UnityEngine;
using UnityEngine.SceneManagement;

public class SaludJugador : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float _vidaMax = 100f;

    [Header("Daño por caída")]
    [SerializeField] private float _distanciaSinDanio = 5.5f;
    [SerializeField] private float _danioPorUnidad = 10f;
    [SerializeField] private float _danioMaximo = 100f;

    public float vida;
    public float VidaMax => _vidaMax;

    private DetectorSuelo _detector;
    private bool _estabaEnSuelo;
    private float _alturaMax;

    private void Awake()
    {
        vida = _vidaMax;
        _detector = GetComponentInChildren<DetectorSuelo>();
        _alturaMax = transform.position.y;
        _estabaEnSuelo = true;
    }

    private void FixedUpdate()
    {
        bool enSuelo = _detector != null && _detector.estaEnSuelo;
        float y = transform.position.y;

        if (enSuelo)
        {
            if (!_estabaEnSuelo)
                Aterrizar(_alturaMax - y);

            _alturaMax = y;
        }
        else
        {
            _alturaMax = Mathf.Max(_alturaMax, y);
        }

        _estabaEnSuelo = enSuelo;
    }

    private void Aterrizar(float distanciaCaida)
    {
        if (distanciaCaida <= _distanciaSinDanio) return;

        float danio = (distanciaCaida - _distanciaSinDanio) * _danioPorUnidad;
        RecibirDanio(Mathf.Min(danio, _danioMaximo));
    }

    public void RecibirDanio(float cantidad)
    {
        vida = Mathf.Max(0f, vida - cantidad);
        Debug.Log("Daño: " + cantidad.ToString("F0") + " | Vida: " + vida.ToString("F0"));

        if (vida <= 0f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}