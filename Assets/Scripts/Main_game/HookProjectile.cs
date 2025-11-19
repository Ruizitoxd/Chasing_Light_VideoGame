using Mirror;
using UnityEngine;

public class HookProjectile : NetworkBehaviour
{
    [HideInInspector] public GrapplingHook_Mirror owner;
    private Vector2 target;
    private float speed;

    public void Initialize(GrapplingHook_Mirror owner, Vector2 target, float speed)
    {
        this.owner = owner;
        this.target = target;
        this.speed = speed;
    }

    private void Update()
    {
        if (!isServer) return;

        // Movimiento del gancho
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        // Si llegó sin pegar a nadie, destruir
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isServer) return;

        if (col.CompareTag("Player"))
        {
            // Evitar enganchar al asesino mismo
            if (col.transform == owner.transform)
                return;

            NetworkIdentity victim = col.GetComponent<NetworkIdentity>();

            if (victim != null)
                owner.Server_HookHit(victim);

            NetworkServer.Destroy(gameObject);
        }
    }
}
