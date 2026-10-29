using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

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

    private bool EnSuelo => _detector != null && _detector.estaEnSuelo && _bloqueoSuelo <= 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gravedad;
        _detector = GetComponentInChildren<DetectorSuelo>();
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        _bloqueoSuelo -= Time.deltaTime;

        _horizontal = 0f;
        if (Input.GetKey(KeyCode.A)) _horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) _horizontal += 1f;

        bool enSuelo = EnSuelo;

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
            }

            if (!_cargando && _horizontal != 0f)
                _sprite.flipX = _horizontal < 0f;
        }
        else
        {
            _cargando = false;
            _carga = 0f;
        }

        _anim.SetBool("corriendo", enSuelo && !_cargando && _horizontal != 0f);
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