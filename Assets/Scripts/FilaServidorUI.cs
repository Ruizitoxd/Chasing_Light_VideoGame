using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class FilaServidorUI : MonoBehaviour
{
    public TMP_Text textoNombre;
    public TMP_Text textoEstado;
    public TMP_Text textoJugadores;

    private ServerResponse info;

    public void Configurar(ServerResponse data)
    {
        info = data;

        textoNombre.text = data.name;
        textoEstado.text = data.isOpen ? "Disponible" : "Ocupado";
        textoJugadores.text = $"{data.players}/{data.maxPlayers}";
    }

    public void Conectar()
    {
        if (info.uri == null)
        {
            Debug.LogError("❌ Error: info.uri es NULL, no se puede conectar.");
            return;
        }

        Debug.Log($"🔌 Conectando a {info.uri.Host}:{info.uri.Port}");

        ((MyLanManager)NetworkManager.singleton).ConnectToServer(info);
    }
}
