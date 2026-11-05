using UnityEngine;

public class GestorPociones : MonoBehaviour
{
    public static GestorPociones instancia;

    public int total = 10;
    public int recogidas = 0;

    void Awake()
    {
        instancia = this;
    }

    public void Recoger()
    {
        recogidas++;
    }

    public bool TodasRecogidas()
    {
        return recogidas >= total;
    }
}