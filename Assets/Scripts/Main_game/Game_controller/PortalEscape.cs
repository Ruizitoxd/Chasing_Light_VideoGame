using UnityEngine;

public class PortalEscape : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerRole p = col.GetComponent<PlayerRole>();

        if (p == null) return; // No es un jugador

        // Solo Supervivientes pueden escapar
        if (p.rol == RolJugador.Superviviente)
        {
            // Esto ya avisa al servidor y actualiza victoria
            if (p.isServer)
            {
                p.Escapar();
            }
        }
    }
}
