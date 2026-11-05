using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject _panelPausa;

    public void Pausar()
    {
        _panelPausa.SetActive(true);
        Time.timeScale = 0f;
        Temporizador.Corriendo = false;
    }

    public void Reanudar()
    {
        _panelPausa.SetActive(false);
        Time.timeScale = 1f;
        Temporizador.Corriendo = true;
    }
}