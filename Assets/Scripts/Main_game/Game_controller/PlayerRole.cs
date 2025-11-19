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
    public GrapplingHook_Mirror  grapplingHook;   // Script del gancho
    public GameObject hookObject;         // El objeto visual del gancho (si tienes uno)

    void OnRolAsignado(RolJugador oldRol, RolJugador newRol)
    {
        // 🔹 Activar el modelo correcto
        modeloAsesino.SetActive(newRol == RolJugador.Asesino);
        modeloSuperviviente.SetActive(newRol == RolJugador.Superviviente);

        // 🔹 Activar el gancho SOLO si es Asesino y SOLO en el jugador local
        if (isLocalPlayer)
        {
            if (grapplingHook != null)
                grapplingHook.enabled = (newRol == RolJugador.Asesino);

            if (hookObject != null)
                hookObject.SetActive(newRol == RolJugador.Asesino);
        }
        else
        {
            // 🔹 Los jugadores remotos NO deben usar su propio gancho
            if (grapplingHook != null)
                grapplingHook.enabled = false;

            if (hookObject != null)
                hookObject.SetActive(newRol == RolJugador.Asesino);
        }
    }

    public override void OnStartClient()
    {
        // Asegurar que el cliente nuevo cargue el modelo correcto
        OnRolAsignado(rol, rol);
    }

    public override void OnStartLocalPlayer()
    {
        // Cuando el jugador local se crea, asegurar que el gancho esté bien configurado
        OnRolAsignado(rol, rol);
    }
}
