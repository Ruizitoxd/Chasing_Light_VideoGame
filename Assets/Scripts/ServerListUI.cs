using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerListUI : MonoBehaviour
{
    public Transform content;  // referencia al Content del ScrollView
    public GameObject serverEntryPrefab; // prefab de la fila

    // Esta lista simula servidores. Luego puedes reemplazarla por Mirror Discovery
    private List<ServerInfo> servidores = new List<ServerInfo>();

    void Start()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        // limpiar antes de agregar
        foreach (Transform child in content)
            Destroy(child.gameObject);

        // ejemplo: datos falsos, luego será autodetectado
        servidores.Clear();
        servidores.Add(new ServerInfo("Servidor 1", 1, 4));
        servidores.Add(new ServerInfo("Servidor 2", 2, 4));
        servidores.Add(new ServerInfo("Servidor PVP", 3, 6));

        // instanciar filas
        foreach (var server in servidores)
        {
            GameObject entry = Instantiate(serverEntryPrefab, content);
            entry.GetComponent<ServerEntryUI>().SetData(server);
        }
    }

    public void GoBack()
    {
        this.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class ServerInfo
{
    public string nombre;
    public int jugadores;
    public int max;

    public ServerInfo(string nombre, int jugadores, int max)
    {
        this.nombre = nombre;
        this.jugadores = jugadores;
        this.max = max;
    }
}
