using UnityEngine;

public class DetectorSuelo : MonoBehaviour
{
    public bool estaEnSuelo;
    [SerializeField] private float _radio = 0.15f;

    private void FixedUpdate()
    {
        estaEnSuelo = false;

        foreach (Collider2D colision in Physics2D.OverlapCircleAll(transform.position, _radio))
        {
            if (colision.CompareTag("Suelo"))
            {
                estaEnSuelo = true;
                break;
            }
        }
    }
}