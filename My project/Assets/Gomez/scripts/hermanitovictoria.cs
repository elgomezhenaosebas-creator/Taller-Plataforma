using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victoria : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _textoTiempo;

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
        if (_gano) return;

        MovimientoJugador mov = other.GetComponentInParent<MovimientoJugador>();
        if (mov == null) return;

        _gano = true;
        Temporizador.Corriendo = false;
        mov.enabled = false;
        mov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        _panel.SetActive(true);
        _textoTiempo.text = "Tiempo: " + Temporizador.Formato(Temporizador.Tiempo);
    }
}