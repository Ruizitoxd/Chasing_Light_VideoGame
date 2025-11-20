using Mirror;
using UnityEngine;

public class KillerHitbox : NetworkBehaviour
{
    public float killDistance = 1.5f;

    private void OnTriggerStay(Collider other)
    {
        if (!isServer) return; // Solo el servidor puede matar

        PlayerRole target = other.GetComponent<PlayerRole>();

        if (target != null && target.rol == RolJugador.Superviviente)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);

            if (dist <= killDistance)
            {
                // Llamamos a la muerte
                target.GetComponent<PlayerHealth>().Kill();
            }
        }
    }
}
