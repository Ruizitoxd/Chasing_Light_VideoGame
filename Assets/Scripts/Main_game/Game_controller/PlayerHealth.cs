using Mirror;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] public bool isDead = false;

    public void Kill()
    {
        if (isDead) return;

        isDead = true;

        // Aquí pon lo que quieras que pase cuando muere
        RpcOnDeath();
    }

    [ClientRpc]
    void RpcOnDeath()
    {
        Debug.Log("Jugador murió");
        // Puedes desactivar modelo, animación, etc
    }
}
