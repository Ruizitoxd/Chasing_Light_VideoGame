using Mirror;
using UnityEngine;

public enum RolJugador { Asesino, Superviviente }

public class PlayerRole : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnRolAsignado))]
    public RolJugador rol;

    [Header("Colores del jugador")]
    public Color32 colorSuperviviente = new Color32(255, 255, 255, 255); // Cian superviviente
    public Color32 colorAsesino = new Color32(255, 255, 0, 121); // Morado asesinio
    private SpriteRenderer spriteRenderer;

    [Header("Arma / Habilidad del Asesino")]
    public GrapplingKnife_Mirror grapplingHook; // Versión apuñalamiento
    public GameObject hookObject;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnRolAsignado(RolJugador oldRol, RolJugador newRol)
    {
        // =============================
        //  ACTIVAR COLORES
        // =============================
        spriteRenderer.color = (newRol == RolJugador.Asesino) ? colorAsesino : colorSuperviviente;

        // =============================
        //  CAMBIAR TAG SEGÚN EL ROL
        // =============================
        if (newRol == RolJugador.Asesino)
            gameObject.tag = "Asesino";
        else
            gameObject.tag = "Player";

        // =============================
        //  CONFIGURAR HERRAMIENTAS DEL ASESINO
        // =============================
        if (newRol == RolJugador.Asesino)
        {
            if (grapplingHook) grapplingHook.enabled = true;
            if (hookObject) hookObject.SetActive(true);
        }
        else
        {
            if (grapplingHook) grapplingHook.enabled = false;
            if (hookObject) hookObject.SetActive(false);
        }
    }

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
}
