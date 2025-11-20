using Mirror;
using UnityEngine;

public enum RolJugador { Asesino, Superviviente }

public class PlayerRole : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnRolAsignado))]
    public RolJugador rol;

    [Header("Modelos del jugador")]
    public GameObject modeloAsesino;
    public GameObject modeloSuperviviente;

    [Header("Arma / Habilidad del Asesino")]
    public GrapplingKnife_Mirror grapplingHook; // Versión apuñalamiento
    public GameObject hookObject;

    void OnRolAsignado(RolJugador oldRol, RolJugador newRol)
    {
        // =============================
        //  ACTIVAR MODELOS
        // =============================
        modeloAsesino.SetActive(newRol == RolJugador.Asesino);
        modeloSuperviviente.SetActive(newRol == RolJugador.Superviviente);

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
