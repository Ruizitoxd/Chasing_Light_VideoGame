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

    void Update()
    {
        // Solo input del jugador local
        if (!isLocalPlayer) return;

        // Aquí verificas si es asesino
        PlayerRole role = GetComponent<PlayerRole>();
        if (role == null || role.rol != RolJugador.Asesino) return;

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 target = GetMouseWorldPosition();
            CmdFireHook(target);
        }
    }

    // --------------------------------------------------------
    // 1) El CLIENTE pide disparar el gancho
    // --------------------------------------------------------
    [Command]
    void CmdFireHook(Vector2 targetPos)
    {
        GameObject hookObj = Instantiate(hookPrefab, firePoint.position, Quaternion.identity);

        HookProjectile hook = hookObj.GetComponent<HookProjectile>();
        hook.Initialize(this, targetPos, hookSpeed);

        NetworkServer.Spawn(hookObj);
    }

    // --------------------------------------------------------
    // 2) EL GANCHO GOLPEÓ AL SUPERVIVIENTE
    // --------------------------------------------------------
    [Server]
    public void Server_HookHit(NetworkIdentity victim)
    {
        if (victim == null) return;

        pulledTarget = victim.transform;
        pulling = true;

        RpcStartPull(victim.netId);
    }

    // --------------------------------------------------------
    // 3) CLIENTES EMPIEZAN ANIMACIÓN DE JALAR
    // --------------------------------------------------------
    [ClientRpc]
    void RpcStartPull(uint victimID)
    {
        NetworkIdentity victim = NetworkClient.spawned[victimID];
        if (victim != null)
            pulledTarget = victim.transform;

        pulling = true;
    }

    // --------------------------------------------------------
    // 4) MOVIMIENTO DEL JALADO – solo servidor mueve
    // --------------------------------------------------------
    private void FixedUpdate()
    {
        if (!isServer) return;

        if (pulling && pulledTarget != null)
        {
            pulledTarget.position = Vector2.MoveTowards(
                pulledTarget.position,
                transform.position,
                pullSpeed * Time.deltaTime
            );

            if (Vector2.Distance(pulledTarget.position, transform.position) < 0.4f)
            {
                pulling = false;
                pulledTarget = null;
            }
        }
    }

    // --------------------------------------------------------
    Vector2 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}
