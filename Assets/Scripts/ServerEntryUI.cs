using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerEntryUI : MonoBehaviour
{
    public TMP_Text nombreTxt;
    public TMP_Text jugadoresTxt;
    public Button botonUnirse;

    private ServerInfo info;

    public void SetData(ServerInfo server)
    {
        info = server;

        nombreTxt.text = server.nombre;
        jugadoresTxt.text = $"{server.jugadores}/{server.max}";

        botonUnirse.onClick.AddListener(Unirse);
    }

    void Unirse()
    {
        Debug.Log("Uniéndose a: " + info.nombre);
        // Aquí iría el NetworkManager.StartClient con la IP correspondiente
    }
}
