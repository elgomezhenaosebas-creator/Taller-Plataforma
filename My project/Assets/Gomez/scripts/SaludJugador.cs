using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SaludJugador : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private float _vidaMax = 100f;

    [Header("Daño por caída")]
    [SerializeField] private float _distanciaSinDanio = 5.5f;
    [SerializeField] private float _danioPorUnidad = 10f;
    [SerializeField] private float _danioMaximo = 100f;

    [Header("Escudos")]
    public int escudos = 0;
    public TextMeshProUGUI textoEscudos;

    [Header("Derrota")]
    [SerializeField] private float _esperaDerrota = 1.2f;

    public float vida;
    public float VidaMax => _vidaMax;

    private DetectorSuelo _detector;
    private PlayerSoundController _sonido;
    private bool _estabaEnSuelo;
    private float _alturaMax;
    private bool _muerto;

    private void Awake()
    {
        vida = _vidaMax;
        _detector = GetComponentInChildren<DetectorSuelo>();
        _sonido = GetComponent<PlayerSoundController>();
        _alturaMax = transform.position.y;
        _estabaEnSuelo = true;
    }

    private void Start()
    {
        ActualizarTexto();
    }

    private void FixedUpdate()
    {
        if (_muerto) return;

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

        // Si hay escudo, absorbe esta caída
        if (escudos > 0)
        {
            escudos--;
            ActualizarTexto();
            Debug.Log("Escudo gastado. Quedan: " + escudos);
            return;
        }

        float danio = (distanciaCaida - _distanciaSinDanio) * _danioPorUnidad;
        RecibirDanio(Mathf.Min(danio, _danioMaximo));
    }

    public void RecibirDanio(float cantidad)
    {
        if (_muerto) return;

        vida = Mathf.Max(0f, vida - cantidad);
        Debug.Log("Daño: " + cantidad.ToString("F0") + " | Vida: " + vida.ToString("F0"));

        if (vida <= 0f)
        {
            StartCoroutine(Morir());
            return;
        }

        if (_sonido != null) _sonido.playRecibirDanio();
    }

    private IEnumerator Morir()
    {
        _muerto = true;

        if (_sonido != null) _sonido.playsonidoDerrota();

        MovimientoJugador mov = GetComponent<MovimientoJugador>();
        if (mov != null) mov.enabled = false;

        yield return new WaitForSeconds(_esperaDerrota);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool Curar(float cantidad)
    {
        if (vida >= _vidaMax)
        {
            escudos++;              // vida llena: la poción da un escudo
            ActualizarTexto();
            return true;            // poción consumida
        }

        vida = Mathf.Min(_vidaMax, vida + cantidad);
        Debug.Log("Curado: " + cantidad.ToString("F0") + " | Vida: " + vida.ToString("F0"));
        return true;
    }

    private void ActualizarTexto()
    {
        if (textoEscudos == null) return;

        textoEscudos.gameObject.SetActive(escudos > 0);
        textoEscudos.text = "Escudos de caída: " + escudos;
    }
}