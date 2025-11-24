using Mirror;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Partida_controller : NetworkBehaviour
{
    public static Partida_controller instancia;

    [Header("Fragmentos")]
    public int CantidadFragmentos = 0;
    public int FragmentosNecesarios = 5;

    [Header("Tiempo")]
    [SyncVar(hook = nameof(OnTiempoChanged))]
    public float TiempoJugado = 0f;
    public float TiempoLimite = 600f; // 10 minutos
    public TMP_Text tiempoTXT;

    [Header("Portal")]
    public GameObject Portal;

    [Header("Jugadores")]
    public List<PlayerRole> supervivientesVivos = new List<PlayerRole>();
    public int supervivientesEscapados = 0;
    public bool partidaFinalizada = false;

    void Awake()
    {
        instancia = this;
    }

    // ============================================================
    // ==================== TIEMPO ================================
    // ============================================================

    void Update()
    {
        if (!isServer || partidaFinalizada) return;

        TiempoJugado += Time.deltaTime;

        // Tiempo agotado → gana asesino
        if (TiempoJugado >= TiempoLimite)
        {
            FinalizarPartida("asesino");
        }
    }

    void OnTiempoChanged(float oldValue, float newValue)
    {
        if (tiempoTXT != null)
        {
            int minutos = Mathf.FloorToInt(newValue / 60f);
            int segundos = Mathf.FloorToInt(newValue % 60f);
            tiempoTXT.text = minutos.ToString("00") + ":" + segundos.ToString("00");
        }
    }

    // ============================================================
    // ========= SISTEMA DE FRAGMENTOS Y PORTAL ====================
    // ============================================================

    [Server]
    public void SumarFragmentos()
    {
        CantidadFragmentos++;
        Debug.Log($"🟦 Fragmento recogido: {CantidadFragmentos}/{FragmentosNecesarios}");

        if (CantidadFragmentos >= FragmentosNecesarios)
            ActivarPortal();
    }

    [Server]
    public void ActivarPortal()
    {
        if (Portal != null)
        {
            Portal.SetActive(true);
            Debug.Log("🟩 PORTAL ACTIVADO — Los supervivientes pueden escapar");
        }
        else
        {
            Debug.LogError("❌ Portal es NULL — ¿Lo asignaste en el inspector?");
        }
    }


    // ============================================================
    // ========= REGISTRO DE SUPERVIVIENTES ========================
    // ============================================================

    // Llamado SOLO desde NetworkManager después de asignar roles
    [Server]
    public void RegistrarSuperviviente(PlayerRole p)
    {
        if (p == null)
        {
            Debug.LogError("❌ Intento de registrar superviviente NULL");
            return;
        }

        if (supervivientesVivos.Contains(p))
        {
            Debug.LogWarning("⚠️ Este superviviente YA está registrado");
            return;
        }

        supervivientesVivos.Add(p);
        Debug.Log($"🟨 Superviviente registrado. Total vivos: {supervivientesVivos.Count}");
    }

    // Llamado cuando muere
    [Server]
    public void SupervivienteMuere(PlayerRole p)
    {
        if (supervivientesVivos.Remove(p))
        {
            Debug.Log($"🟥 Un superviviente murió. Quedan vivos: {supervivientesVivos.Count}");
        }

        if (supervivientesVivos.Count == 0)
        {
            Debug.Log("☠️ No quedan supervivientes → gana ASesino");
            FinalizarPartida("asesino");
        }
    }

    // Llamado cuando escapa
    [Server]
    public void SupervivienteEscapa(PlayerRole p)
    {
        if (!supervivientesVivos.Contains(p))
        {
            Debug.LogWarning("⚠️ Un jugador intentó escapar pero NO era un superviviente vivo");
            return;
        }

        supervivientesVivos.Remove(p);
        supervivientesEscapados++;

        Debug.Log($"🟦 Superviviente escapó. Escapados: {supervivientesEscapados}");

        if (supervivientesVivos.Count == 0)
        {
            Debug.Log("🏁 Todos los supervivientes vivos escaparon → GANAN SUPERVIVIENTES");
            FinalizarPartida("supervivientes");
        }
    }


    // ============================================================
    // ================= FIN DE PARTIDA ===========================
    // ============================================================

    [Server]
    public void FinalizarPartida(string ganador)
    {
        if (partidaFinalizada)
        {
            Debug.LogWarning("⚠️ FinalizarPartida() llamado dos veces");
            return;
        }

        partidaFinalizada = true;

        Debug.Log("🎉 FINAL DE PARTIDA — Ganador: " + ganador);

        RpcMostrarPantallaFinal(ganador);

        // Aquí puedes congelar jugadores, deshabilitar controles, etc.
    }

    [ClientRpc]
    void RpcMostrarPantallaFinal(string ganador)
    {
        Debug.Log($"🏆 GANADOR: {ganador.ToUpper()}");
        // Aquí puedes activar tu UI de victoria
    }
}
