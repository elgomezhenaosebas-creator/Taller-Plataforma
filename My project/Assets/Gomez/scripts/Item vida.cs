using UnityEngine;

public class ItemVida : MonoBehaviour
{
    [SerializeField] private float _cura = 25f;

    private bool _recogida;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_recogida) return;

        SaludJugador salud = other.GetComponentInParent<SaludJugador>();
        if (salud == null) return;

        _recogida = true;

        salud.Curar(_cura);   // cura, o da escudo si ya tenía la vida llena

        // Sonido de poción recogida
        PlayerSoundController sonido = other.GetComponentInParent<PlayerSoundController>();
        if (sonido != null) sonido.playsonidopocionRecogida();

        if (GestorPociones.instancia != null)
            GestorPociones.instancia.Recoger();
        else
            Debug.LogWarning("Falta el objeto GestorPociones en la escena");

        Destroy(gameObject);
    }
}