using Mirror;
using UnityEngine;

public class GrapplingHook_Mirror : NetworkBehaviour
{
    [Header("Hook Settings")]
    public GameObject hookPrefab;
    public Transform firePoint;
    public float hookSpeed = 20f;
    public float pullSpeed = 10f;

    private Transform pulledTarget;
    private bool pulling = false;

    private GameObject spawnedHook;

    // Referencia al asesino en el servidor
    private NetworkIdentity ownerIdentity;

    // ===========================================================
    // Asignamos ownerIdentity en servidor al iniciar
    // ===========================================================
    public override void OnStartServer()
    {
        base.OnStartServer();
        ownerIdentity = GetComponent<NetworkIdentity>();
    }

    void Update()
    {
        // Solo el jugador local puede disparar el gancho
        if (!isLocalPlayer) return;

        // Solo el Asesino puede usar el gancho
        PlayerRole role = GetComponent<PlayerRole>();
        if (role == null || role.rol != RolJugador.Asesino) return;

        if (Input.GetMouseButtonDown(1)) // click derecho
        {
            Vector2 target = GetMouseWorldPosition();
            CmdFireHook(target);
        }
    }

    // ===========================================================
    // 1) CLIENTE -> SERVIDOR : Disparar gancho
    // ===========================================================
    [Command]
    void CmdFireHook(Vector2 targetPos)
    {
        // Instanciar el gancho en el Servidor
        spawnedHook = Instantiate(hookPrefab, firePoint.position, Quaternion.identity);

        HookProjectile hook = spawnedHook.GetComponent<HookProjectile>();
        hook.Initialize(this, targetPos, hookSpeed);

        NetworkServer.Spawn(spawnedHook);
    }

    // ===========================================================
    // 2) Cuando el gancho golpea a un jugador
    // ===========================================================
    [Server]
    public void Server_HookHit(NetworkIdentity victim)
    {
        if (victim == null) return;

        pulledTarget = victim.transform;
        pulling = true;

        // No necesitamos reasignar ownerIdentity, ya está en OnStartServer()

        RpcStartPull(victim.netId);

        if (spawnedHook != null)
        {
            NetworkServer.Destroy(spawnedHook);
            spawnedHook = null;
        }
    }

    // ===========================================================
    // 3) RPC -> Todos ven que empieza el jalado
    // ===========================================================
    [ClientRpc]
    void RpcStartPull(uint victimID)
    {
        if (NetworkClient.spawned.TryGetValue(victimID, out NetworkIdentity victim))
        {
            pulledTarget = victim.transform;
            pulling = true;
        }
    }

    // ===========================================================
    // 4) EL SERVIDOR mueve al objetivo
    // ===========================================================
    private void FixedUpdate()
    {
        if (!isServer) return; // mover solo en servidor

        if (pulling && pulledTarget != null && ownerIdentity != null)
        {
            Rigidbody2D rb = pulledTarget.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("El jugador no tiene Rigidbody2D");
                pulling = false;
                return;
            }

            // Dirección hacia el asesino
            Vector2 direction = (ownerIdentity.transform.position - pulledTarget.position).normalized;
            rb.velocity = direction * pullSpeed;

            // Si ya llegó
            if (Vector2.Distance(pulledTarget.position, ownerIdentity.transform.position) < 0.5f)
            {
                rb.velocity = Vector2.zero;
                pulling = false;
                pulledTarget = null;
            }
        }
    }

    // ===========================================================
    // RPCs de QTE (no se modifican)
    // ===========================================================
    [TargetRpc]
    void TargetStartQTE(NetworkConnection target)
    {
        QTE_UI.Instance.StartKillerQTE();
    }

    [TargetRpc]
    void TargetStartQTE_Victim(NetworkConnection target)
    {
        QTE_UI.Instance.StartVictimQTE();
    }

    // ----------------------------------------------------------
    Vector2 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}
