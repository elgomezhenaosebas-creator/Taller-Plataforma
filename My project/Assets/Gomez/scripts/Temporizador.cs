using TMPro;
using UnityEngine;

public class Temporizador : MonoBehaviour
{
    public static float Tiempo;
    public static bool Corriendo = true;

    [SerializeField] private TMP_Text _texto;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        Tiempo = 0f;
        Corriendo = true;
    }

    private void Update()
    {
        if (Corriendo) Tiempo += Time.deltaTime;
        _texto.text = Formato(Tiempo);
    }

    public static string Formato(float t)
    {
        int min = (int)(t / 60f);
        int seg = (int)(t % 60f);
        int cent = (int)((t * 100f) % 100f);
        return $"{min:00}:{seg:00}.{cent:00}";
    }
}