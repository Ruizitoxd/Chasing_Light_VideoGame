using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [Header("Configuración de roles")]
    public int minPlayersToStart = 2;
    public int maxPlayersAllowed = 5;

    // Lista de todos los jugadores conectados (PlayerRole)
    [HideInInspector] 
    public List<PlayerRole> jugadores = new List<PlayerRole>();


    // ============================================================
    // == AL CONECTAR UN JUGADOR ==================================
    // ============================================================

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("🔵 OnServerAddPlayer() llamado");

        base.OnServerAddPlayer(conn);

        PlayerRole pj = conn.identity.GetComponent<PlayerRole>();

        if (pj != null)
        {
            jugadores.Add(pj);
            Debug.Log($"🟢 Jugador agregado. Total: {jugadores.Count}");
        }
        else
        {
            Debug.LogError("❌ El prefab del jugador NO tiene PlayerRole");
        }

        if (jugadores.Count > maxPlayersAllowed)
        {
            Debug.LogWarning("⚠️ Se superó el máximo de jugadores permitidos");
        }
    }


    // ============================================================
    // == AL DESCONECTARSE UN JUGADOR =============================
    // ============================================================

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("🔴 OnServerDisconnect() llamado");

        if (conn.identity != null)
        {
            PlayerRole pj = conn.identity.GetComponent<PlayerRole>();

            if (pj != null && jugadores.Contains(pj))
            {
                jugadores.Remove(pj);
                Debug.Log($"🟡 Jugador eliminado. Quedan: {jugadores.Count}");
            }
        }

        base.OnServerDisconnect(conn);
    }


    // ============================================================
    // == SERVIDOR DETENIDO =======================================
    // ============================================================

    public override void OnStopServer()
    {
        Debug.Log("🧹 Limpiando lista de jugadores (OnStopServer)");
        jugadores.Clear();
        base.OnStopServer();
    }


    // ============================================================
    // == ASIGNACIÓN DE ROLES =====================================
    // ============================================================

    [Server]
    public void AsignarRoles()
    {
        Debug.Log("🎲 AsignarRoles() llamado…");

        if (jugadores == null)
            jugadores = new List<PlayerRole>();

        Debug.Log($"👥 Jugadores detectados: {jugadores.Count}");

        if (jugadores.Count < minPlayersToStart)
        {
            Debug.LogWarning($"⛔ No hay suficientes jugadores ({jugadores.Count}/{minPlayersToStart})");
            return;
        }

        // Elegir Asesino al azar
        int indexAsesino = Random.Range(0, jugadores.Count);
        Debug.Log($"🔪 Index asesino: {indexAsesino}");

        // Asignar roles
        for (int i = 0; i < jugadores.Count; i++)
        {
            PlayerRole pj = jugadores[i];

            if (pj == null)
            {
                Debug.LogWarning($"⚠️ Jugador null en índice {i}");
                continue;
            }

            if (i == indexAsesino)
            {
                pj.rol = RolJugador.Asesino;
                Debug.Log($"➡️ Jugador {i} → ASESINO");
            }
            else
            {
                pj.rol = RolJugador.Superviviente;
                Debug.Log($"➡️ Jugador {i} → SUPERVIVIENTE");

                // REGISTRARLO EN EL CONTROLADOR
                Partida_controller.instancia.RegistrarSuperviviente(pj);
            }
        }

        Debug.Log("✅ Roles asignados correctamente.");
    }


    // ============================================================
    // == BOTÓN PARA INICIAR PARTIDA ===============================
    // ============================================================

    [Server]
    public void IntentarIniciarYAsignarRoles()
    {
        Debug.Log("🚀 IntentarIniciarYAsignarRoles() llamado");

        if (jugadores.Count >= minPlayersToStart)
        {
            Debug.Log("🟢 Jugadores suficientes. Iniciando…");
            AsignarRoles();
        }
        else
        {
            Debug.LogWarning($"⛔ No se puede iniciar: {jugadores.Count}/{minPlayersToStart} jugadores.");
        }
    }
}
