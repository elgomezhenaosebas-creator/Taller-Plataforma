using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar()
    {
        Temporizador.Tiempo = 0f;
        Temporizador.Corriendo = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir()
    {
        Application.Quit();
    }
}