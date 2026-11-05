using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelDerrota : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private void Awake()
    {
        Time.timeScale = 1f;
        _panel.SetActive(false);
    }

    public void Mostrar()
    {
        Temporizador.Corriendo = false;
        _panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        Temporizador.Tiempo = 0f;
        Temporizador.Corriendo = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        Temporizador.Tiempo = 0f;
        Temporizador.Corriendo = true;
        SceneManager.LoadScene(0);
    }
}