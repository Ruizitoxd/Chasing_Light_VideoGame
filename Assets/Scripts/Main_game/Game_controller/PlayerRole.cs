using Mirror;
using UnityEngine;
using Cainos.PixelArtTopDown_Basic;

public enum RolJugador { Asesino, Superviviente }

public class PlayerRole : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnRolAsignado))]
    public RolJugador rol;

    [Header("Colores del jugador")]
    public Color32 colorSuperviviente = new Color32(255, 255, 255, 255);
    public Color32 colorAsesino = new Color32(255, 255, 0, 121);
    private SpriteRenderer spriteRenderer;

    [Header("Arma / Habilidad del Asesino")]
    public GrapplingKnife_Mirror grapplingHook;
    public GameObject hookObject;
    public TopDownCharacterController characterController;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ============================================================
    // == LOG DE SPAWN EN SERVIDOR ================================
    // ============================================================

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log($"[SERVER] Player SPAWNED con rol inicial = {rol}");
    }

    // ============================================================
    // == HOOK DEL ROL ============================================
    // ============================================================

    void OnRolAsignado(RolJugador oldRol, RolJugador newRol)
    {
        Debug.Log($"[SERVER/CLIENT] HOOK ejecutado: rol cambia de {oldRol} → {newRol} (isServer={isServer})");

        // Actualizar color
        if (spriteRenderer != null)
            spriteRenderer.color = (newRol == RolJugador.Asesino) ? colorAsesino : colorSuperviviente;

        // Tag
        gameObject.tag = (newRol == RolJugador.Asesino) ? "Asesino" : "Player";

        // Arma del asesino
        bool esAsesino = newRol == RolJugador.Asesino;
        if (esAsesino)
        {
            characterController.speed *= 1.2f; // El asesino es un 20% más rápido
        }

        if (grapplingHook) grapplingHook.enabled = esAsesino;
        if (hookObject) hookObject.SetActive(esAsesino);

        // ❌ NO REGISTRAMOS SUPERVIVIENTES AQUÍ
        // El registro AHORA lo maneja el NetworkManager (mucho más seguro).
    }

    // ============================================================
    // == CLIENT INIT =============================================
    // ============================================================

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnRolAsignado(rol, rol);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        OnRolAsignado(rol, rol);
    }

    // ============================================================
    // == MUERTE / ESCAPE =========================================
    // ============================================================

    [Server]
    public void Morir()
    {
        Debug.Log($"[SERVER] Superviviente MUERE: netId={netId}");
        Partida_controller.instancia.SupervivienteMuere(this);
        gameObject.SetActive(false);
    }

    [Server]
    public void Escapar()
    {
        Debug.Log($"[SERVER] Superviviente ESCAPA: netId={netId}");
        Partida_controller.instancia.SupervivienteEscapa(this);
        gameObject.SetActive(false);
    }

    // ============================================================
    // == RECOGER FRAGMENTO =======================================
    // ============================================================

    [Command(requiresAuthority = false)]
    public void CmdRecogerFragmento(GameObject fragmento)
    {
        Debug.Log($"[SERVER] Fragmento recogido por {netId}");
        Partida_controller.instancia.SumarFragmentos();

        if (fragmento != null)
            NetworkServer.Destroy(fragmento);
    }
}
