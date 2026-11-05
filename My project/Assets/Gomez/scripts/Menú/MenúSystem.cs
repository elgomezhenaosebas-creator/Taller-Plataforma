using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private string _escenaJuego = "SampleScene";

    public void Jugar()
    {
        Time.timeScale = 1f;
        Temporizador.Tiempo = 0f;
        Temporizador.Corriendo = true;
        SceneManager.LoadScene(_escenaJuego);
    }

    public void Salir()
    {
        Application.Quit();
    }
}