using UnityEngine;
using UnityEngine.UI;

public class VolumenMusica : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Slider _slider;

    private void Start()
    {
        float guardado = PlayerPrefs.GetFloat("volumenMusica", 0.3f);
        _audio.volume = guardado;
        _slider.value = guardado;
        _slider.onValueChanged.AddListener(Cambiar);
    }

    private void Cambiar(float valor)
    {
        _audio.volume = valor;
        PlayerPrefs.SetFloat("volumenMusica", valor);
    }
}