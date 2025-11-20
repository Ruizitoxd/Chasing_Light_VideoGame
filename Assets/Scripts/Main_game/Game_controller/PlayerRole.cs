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
    public GrapplingHook_Mirror grapplingHook;
    public GameObject hookObject;

    [Header("Sistemas del Superviviente")]
    public PlayerHealth playerHealth;

    [Header("Sistemas del Asesino")]
    public GameObject killerHitbox;



    void OnRolAsignado(RolJugador oldRol, RolJugador newRol)
    {
        // =============================
        //  ACTIVAR MODELADOS
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
        //  CONFIGURAR GANCHO
        // =============================
        if (newRol == RolJugador.Asesino)
        {
            if (isLocalPlayer)
            {
                if (grapplingHook) grapplingHook.enabled = true;
                if (hookObject) hookObject.SetActive(true);
            }
            else if (isServer)
            {
                if (grapplingHook) grapplingHook.enabled = true;
                if (hookObject) hookObject.SetActive(true);
            }
            else
            {
                if (grapplingHook) grapplingHook.enabled = false;
                if (hookObject) hookObject.SetActive(true);
            }
        }
        else
        {
            if (grapplingHook) grapplingHook.enabled = false;
            if (hookObject) hookObject.SetActive(false);
        }

        // =============================
        //  CONFIGURAR HABILIDADES
        // =============================
        if (newRol == RolJugador.Asesino)
        {
            // Activa hitbox de asesino
            if (killerHitbox) killerHitbox.SetActive(true);

            // Desactiva vida del asesino
            if (playerHealth) playerHealth.enabled = false;
        }
        else // Superviviente
        {
            // Activa vida
            if (playerHealth) playerHealth.enabled = true;

            // Desactiva hitbox del asesino
            if (killerHitbox) killerHitbox.SetActive(false);
        }
    }

    public override void OnStartClient()
    {
        OnRolAsignado(rol, rol);
    }

    public override void OnStartLocalPlayer()
    {
        OnRolAsignado(rol, rol);
    }
}
