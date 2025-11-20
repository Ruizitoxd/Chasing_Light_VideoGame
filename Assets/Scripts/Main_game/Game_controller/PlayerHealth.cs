using Mirror;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHealthChanged))]
    public int health = 100;

    public int maxHealth = 100;

    public bool isDead = false;

    public void TakeDamage(int amount)
    {
        if (!isServer || isDead) return;

        health -= amount;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void OnHealthChanged(int oldValue, int newValue)
    {
        // Aquí puedes actualizar UI si quieres
    }

    [Server]
    void Die()
    {
        isDead = true;

        // Desactivar al jugador al morir
        RpcOnDeath();
    }

    [ClientRpc]
    void RpcOnDeath()
    {
        gameObject.SetActive(false);
    }

    [Server]
    public void Kill()
    {
        if (isDead) return;
        health = 0;
        Die();
    }

}
