using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [SerializeField] private float _velocidad = 6f;
    [SerializeField] private float _fuerzaSalto = 12f;

    private Rigidbody2D _rb;
    private DetectorSuelo _detector;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private float _horizontal;
    private bool _saltar;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _detector = GetComponentInChildren<DetectorSuelo>();
        _anim = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        _horizontal = 0f;
        if (Input.GetKey(KeyCode.A)) _horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) _horizontal += 1f;

        if (Input.GetKeyDown(KeyCode.Space))
            _saltar = true;

        if (_horizontal != 0f)
            _sprite.flipX = _horizontal < 0f;

        _anim.SetBool("corriendo", _horizontal != 0f);
        _anim.SetBool("enSuelo", _detector != null && _detector.estaEnSuelo);
    }

    private void FixedUpdate()
    {
        Vector2 v = _rb.linearVelocity;
        v.x = _horizontal * _velocidad;

        if (_saltar && _detector != null && _detector.estaEnSuelo)
            v.y = _fuerzaSalto;

        _saltar = false;
        _rb.linearVelocity = v;
    }
}