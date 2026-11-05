using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _textoTiempo;
    [SerializeField] private GameObject _avisoPociones;

    private bool _gano;

    private void Update()
    {
        if (_gano && Input.GetKeyDown(KeyCode.R))
        {
            Temporizador.Tiempo = 0f;
            Temporizador.Corriendo = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Victoria: toca " + other.name);

        if (_gano) return;

        MovimientoJugador mov = other.GetComponentInParent<MovimientoJugador>();
        if (mov == null) return;

        // Si faltan pociones, avisa y no deja ganar
        if (!GestorPociones.instancia.TodasRecogidas())
        {
            MostrarAviso();
            return;
        }

        _gano = true;
        Temporizador.Corriendo = false;
        mov.enabled = false;
        mov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        PlayerSoundController sonido = mov.GetComponent<PlayerSoundController>();
        if (sonido != null) sonido.playsonidoVictoria();

        _panel.SetActive(true);
        _textoTiempo.text = "Tiempo: " + Temporizador.Formato(Temporizador.Tiempo);
    }

    private void MostrarAviso()
    {
        if (_avisoPociones == null) return;

        _avisoPociones.SetActive(true);
        CancelInvoke(nameof(OcultarAviso));
        Invoke(nameof(OcultarAviso), 2f);
    }

    private void OcultarAviso()
    {
        _avisoPociones.SetActive(false);
    }
}