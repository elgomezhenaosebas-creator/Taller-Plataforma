using UnityEngine;

public class ItemVida : MonoBehaviour
{
    [SerializeField] private float _cura = 25f;

    private void OnTriggerStay2D(Collider2D other)
    {
        SaludJugador salud = other.GetComponentInParent<SaludJugador>();
        if (salud == null) return;

        if (salud.Curar(_cura))
            Destroy(gameObject);
    }
}
