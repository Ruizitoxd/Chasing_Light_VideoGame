using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [Header("Configuración de roles")]
    [Tooltip("Mínimo de jugadores para iniciar la partida")]
    public int minPlayersToStart = 2;

    [Tooltip("Máximo de jugadores permitidos en la partida")]
    public int maxPlayersAllowed = 5;

    // Lista de PlayerRole en el servidor
    [HideInInspector]
    public List<PlayerRole> jugadores = new List<PlayerRole>();


    // Cuando un jugador se añade al servidor
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("🔵 OnServerAddPlayer() llamado (nuevo jugador entrando)");

        base.OnServerAddPlayer(conn);

        PlayerRole pj = conn.identity.GetComponent<PlayerRole>();

        if (pj != null)
        {
            jugadores.Add(pj);
            Debug.Log($"🟢 Jugador agregado a la lista. Total jugadores: {jugadores.Count}");
        }
        else
        {
            Debug.LogWarning("⚠️ El prefab NO tiene PlayerRole. NO se puede asignar rol.");
        }

        if (jugadores.Count > maxPlayersAllowed)
        {
            Debug.LogWarning("⚠️ Se superó el máximo permitido de jugadores.");
        }
    }


    // Remover jugador si se desconecta
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("🔴 OnServerDisconnect() llamado (jugador salió)");

        if (conn.identity != null)
        {
            PlayerRole pj = conn.identity.GetComponent<PlayerRole>();
            if (pj != null && jugadores.Contains(pj))
            {
                jugadores.Remove(pj);
                Debug.Log($"🟡 Jugador eliminado de la lista. Jugadores restantes: {jugadores.Count}");
            }
        }

        base.OnServerDisconnect(conn);
    }


    // Limpiar al detener servidor
    public override void OnStopServer()
    {
        Debug.Log("🧹 Servidor detenido. Limpiando lista de jugadores...");
        jugadores.Clear();
        base.OnStopServer();
    }


    // Método para asignar roles
    [Server]
    public void AsignarRoles()
    {
        Debug.Log("🎲 AsignarRoles() llamado…");

        if (jugadores == null) jugadores = new List<PlayerRole>();

        Debug.Log($"👥 Jugadores actuales: {jugadores.Count}");

        if (jugadores.Count < minPlayersToStart)
        {
            Debug.LogWarning($"⛔ No hay suficientes jugadores ({jugadores.Count}/{minPlayersToStart})");
            return;
        }

        int indexAsesino = Random.Range(0, jugadores.Count);
        Debug.Log($"🔪 Asesino elegido aleatoriamente: index {indexAsesino}");

        for (int i = 0; i < jugadores.Count; i++)
        {
            if (jugadores[i] == null)
            {
                Debug.LogWarning($"⚠️ Jugador null en índice {i}");
                continue;
            }

            if (i == indexAsesino)
            {
                jugadores[i].rol = RolJugador.Asesino;
                Debug.Log($"➡️ Jugador {i}: rol ASIGNADO = ASESINO");
            }
            else
            {
                jugadores[i].rol = RolJugador.Superviviente;
                Debug.Log($"➡️ Jugador {i}: rol ASIGNADO = SUPERVIVIENTE");
            }
        }

        Debug.Log("✅ Roles asignados completamente.");
    }


    // Método utilitario para iniciar partida desde un botón
    [Server]
    public void IntentarIniciarYAsignarRoles()
    {
        Debug.Log("🚀 IntentarIniciarYAsignarRoles() llamado.");

        if (jugadores.Count >= minPlayersToStart)
        {
            Debug.Log($"🟢 Jugadores suficientes ({jugadores.Count}). Iniciando asignación…");
            AsignarRoles();
        }
        else
        {
            Debug.LogWarning($"⛔ No se puede iniciar partida: {jugadores.Count}/{minPlayersToStart} jugadores.");
        }
    }
}
