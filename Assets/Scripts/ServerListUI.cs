using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;

public class ServerListUI : MonoBehaviour
{
    [Header("UI")]
    public Transform content;
    public GameObject filaPrefab;

    Dictionary<long, ServerResponse> servidores = new();

    public void LimpiarLista()
    {
        foreach (Transform hijo in content)
            Destroy(hijo.gameObject);

        servidores.Clear();
    }

    public void AgregarServidor(ServerResponse info)
    {
        if (servidores.ContainsKey(info.serverId)) return;

        servidores[info.serverId] = info;

        GameObject filaGO = Instantiate(filaPrefab, content);

        // Obtiene el componente del prefab
        var filaUI = filaGO.GetComponent<FilaServidorUI>();

        // Configura la fila con la información del servidor
        filaUI.Configurar(info);

        // Botón llama a Conectar()
        filaGO.GetComponent<Button>().onClick.AddListener(() =>
        {
            filaUI.Conectar();  
        });
    }
}
