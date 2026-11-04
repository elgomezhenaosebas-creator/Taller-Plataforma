using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _velocidad = 6f;
    [SerializeField] private float _gravedad = 3f;

    [Header("Salto")]
    [SerializeField] private float _saltoMin = 9f;
    [SerializeField] private float _saltoMax = 17f;
    [SerializeField] private float _tiempoCarga = 1f;
    [SerializeField] private float _empujeHorizontal = 4f;

    [Header("Rebote en paredes")]
    [SerializeField] private float _rebote = 0.6f;

    [Header("Sonidos")]
    [SerializeField] private float _intervaloPaso = 0.3f;
    [SerializeField] private float _velocidadCaidaMin = 8f;

    private PlayerSoundController _sonido;
    private Rigidbody2D _rb;
    private DetectorSuelo _detector;
    private Animator _anim;
    private SpriteRenderer _sprite;

    private float _horizontal;
    private float _carga;
    private bool _cargando;
    private bool _lanzar;
    private float _fuerzaPendiente;
    private float _dirPendiente;
    private float _bloqueoSuelo;
    private Vector2 _velPrevia;

    private bool _estabaEnSuelo;
    private float _velCaidaMax;
    private float _tempoPaso;
    private bool _pasoAlterno;

    private bool EnSuelo => _detector != null && _detector.estaEnSuelo && _bloqueoSuelo <= 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gravedad;
        _detector = GetComponentInChildren<DetectorSuelo>();
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _sonido = GetComponent<PlayerSoundController>();
    }

    private void Update()
    {
        _bloqueoSuelo -= Time.deltaTime;

        _horizontal = 0f;
        if (Input.GetKey(KeyCode.A)) _horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) _horizontal += 1f;

        bool enSuelo = EnSuelo;

        // Sonido de caída al aterrizar
        if (enSuelo && !_estabaEnSuelo && _velCaidaMax < -_velocidadCaidaMin)
        {
            if (_sonido != null) _sonido.playsonidoCaida();
        }

        if (enSuelo) _velCaidaMax = 0f;
        else _velCaidaMax = Mathf.Min(_velCaidaMax, _rb.linearVelocity.y);

        _estabaEnSuelo = enSuelo;

        if (enSuelo)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _cargando = true;
                _carga = Mathf.Min(_carga + Time.deltaTime, _tiempoCarga);
            }

            if (_cargando && (Input.GetKeyUp(KeyCode.Space) || _carga >= _tiempoCarga))
            {
                _fuerzaPendiente = Mathf.Lerp(_saltoMin, _saltoMax, _carga / _tiempoCarga);
                _dirPendiente = _horizontal;
                _lanzar = true;
                _cargando = false;
                _carga = 0f;

                if (_sonido != null) _sonido.playSaltar();
            }

            if (!_cargando && _horizontal != 0f)
                _sprite.flipX = _horizontal < 0f;
        }
        else
        {
            _cargando = false;
            _carga = 0f;
        }

        bool caminando = enSuelo && !_cargando && _horizontal != 0f;

        // Pasos alternando sonido 1 y 2
        if (caminando)
        {
            _tempoPaso -= Time.deltaTime;
            if (_tempoPaso <= 0f)
            {
                _tempoPaso = _intervaloPaso;
                if (_sonido != null)
                {
                    if (_pasoAlterno) _sonido.playsonidoMov2();
                    else _sonido.playsonidoMov1();
                }
                _pasoAlterno = !_pasoAlterno;
            }
        }
        else
        {
            _tempoPaso = 0f;
        }

        _anim.SetBool("corriendo", caminando);
        _anim.SetBool("enSuelo", enSuelo);
    }

    private void FixedUpdate()
    {
        _velPrevia = _rb.linearVelocity;

        if (_lanzar)
        {
            _rb.linearVelocity = new Vector2(_dirPendiente * _empujeHorizontal, _fuerzaPendiente);
            _lanzar = false;
            _bloqueoSuelo = 0.15f;
            return;
        }

        if (EnSuelo)
        {
            Vector2 v = _rb.linearVelocity;
            v.x = _cargando ? 0f : _horizontal * _velocidad;
            _rb.linearVelocity = v;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (EnSuelo || Mathf.Abs(_velPrevia.x) < 0.5f) return;

        foreach (ContactPoint2D c in col.contacts)
        {
            if (Mathf.Abs(c.normal.x) > 0.7f)
            {
                float nuevaX = -_velPrevia.x * _rebote;
                _rb.linearVelocity = new Vector2(nuevaX, _rb.linearVelocity.y);
                _sprite.flipX = nuevaX < 0f;
                break;
            }
        }
    }
}