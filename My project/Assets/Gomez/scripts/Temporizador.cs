using TMPro;
using UnityEngine;

public class Temporizador : MonoBehaviour
{
    public static float Tiempo;
    public static bool Corriendo = true;

    [SerializeField] private TMP_Text _texto;

    private static int _ultimoFrame = -1;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        Tiempo = 0f;
        Corriendo = true;
        _ultimoFrame = -1;
    }

    private void Update()
    {
        if (Corriendo && _ultimoFrame != Time.frameCount)
        {
            _ultimoFrame = Time.frameCount;
            Tiempo += Time.deltaTime;
        }

        if (_texto != null)
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