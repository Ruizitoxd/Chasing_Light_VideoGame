using Mirror;
using UnityEngine;

public class HookProjectile : NetworkBehaviour
{
    private GrapplingHook_Mirror owner;
    private Vector2 targetPos;
    private float speed;
    private bool hitSomething = false;

    // Inicializar el gancho con el dueño y velocidad
    public void Initialize(GrapplingHook_Mirror owner, Vector2 targetPos, float speed)
    {
        this.owner = owner;
        this.targetPos = targetPos;
        this.speed = speed;
    }

    void Update()
    {
        if (!isServer || hitSomething) return;

        // Mover gancho hacia targetPos
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        // Destruir si llegó al target sin golpear
        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isServer || hitSomething) return;

        // Solo interactuar con jugadores
        if (other.CompareTag("Player"))
        {
            NetworkIdentity victim = other.GetComponent<NetworkIdentity>();

            // Evitar engancharse a sí mismo
            if (victim != null && owner != null && victim.netId != owner.GetComponent<NetworkIdentity>().netId)
            {
                hitSomething = true; // detiene el movimiento

                // Avisar al servidor que se enganchó
                owner.Server_HookHit(victim);

                // Apagar visual del gancho para todos los clientes
                RpcDisableVisual();

                // Destruir gancho un poco después para asegurar que RPC se procese
                Invoke(nameof(Server_DestroySelf), 0.1f);
            }
        }
    }

    [ClientRpc]
    void RpcDisableVisual()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }

    [Server]
    void Server_DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
