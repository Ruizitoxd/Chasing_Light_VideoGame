using Mirror;
using UnityEngine;

public class GrapplingKnife_Mirror : NetworkBehaviour
{
    [Header("Knife Settings")]
    public float attackRange = 1.5f;       // Rango de apuñalamiento
    public int damage = 100;               // Daño al superviviente
    public LayerMask hitLayers;            // Layer de los supervivientes

    [Header("Visual Effect")]
    public GameObject stabEffect;          // Prefab del efecto de golpe

    void Update()
    {
        if (!isLocalPlayer) return;       // Solo el jugador local controla el ataque

        // Verifica si es asesino
        PlayerRole role = GetComponent<PlayerRole>();
        if (role == null || role.rol != RolJugador.Asesino) return;

        // Click izquierdo para atacar
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;  // Dirección del ataque

        // Debug: dibuja el raycast en Scene view
        Debug.DrawRay(origin, direction * attackRange, Color.red, 1f);

        // Raycast hacia la dirección del asesino
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, attackRange, hitLayers);

        if (hit.collider != null)
        {
            NetworkIdentity victimID = hit.collider.GetComponent<NetworkIdentity>();

            if (victimID != null && victimID != netIdentity)
            {
                CmdAttack(victimID);           // Comando al servidor
                PlayStabEffect(hit.point);     // Efecto visual
            }
        }
    }

    [Command]
    void CmdAttack(NetworkIdentity victim)
    {
        if (victim == null) return;

        PlayerHealth hp = victim.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage);           // Aplica daño server-authoritative
        }
    }

    // --------------------------
    // Método para mostrar efecto visual
    // --------------------------
    void PlayStabEffect(Vector2 position)
    {
        if (stabEffect != null)
        {
            // Desplaza un poco el efecto hacia delante del jugador golpeado
            Vector2 spawnPos = position + (Vector2)transform.right * 0.3f;
            GameObject fx = Instantiate(stabEffect, spawnPos, Quaternion.identity);
            Destroy(fx, 0.5f);   // Dura medio segundo
        }
    }
}
