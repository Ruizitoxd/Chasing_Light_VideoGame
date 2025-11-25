using UnityEngine;
using Mirror;

public class BotonBuscarUI : MonoBehaviour
{
    public void Buscar()
    {
        Debug.Log("Botón BUSCAR presionado");

        var lan = (MyLanManager)NetworkManager.singleton;

        // Reiniciar descubrimiento
        lan.discovery.StopDiscovery();  // evita duplicados
        lan.discovery.StartDiscovery(); // envía automáticamente la búsqueda

        Debug.Log("🛰 Buscando servidores LAN...");
    }
}
