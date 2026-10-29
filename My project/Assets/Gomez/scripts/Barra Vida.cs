using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] private SaludJugador _salud;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _velocidad = 2f;

    private void Awake()
    {
        if (_slider == null) _slider = GetComponent<Slider>();
        _slider.value = 1f;
    }

    private void Update()
    {
        if (_salud == null) return;

        float objetivo = _salud.vida / _salud.VidaMax;
        _slider.value = Mathf.MoveTowards(_slider.value, objetivo, _velocidad * Time.deltaTime);
    }
}