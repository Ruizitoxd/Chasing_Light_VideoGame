using UnityEngine;
using Mirror;

public class MyLanManager : NetworkManager
{
    public string nombreServidor = "Servidor LAN";
    public MyLanDiscovery discovery;

    [Header("Escenas")]
    public string mainScene = "Main_game";

    // ============================
    // HOST INICIA SERVIDOR
    // ============================
    public override void OnStartHost()
    {
        base.OnStartHost();

        Debug.Log("HOST creado. Anunciando servidor...");
        discovery.AdvertiseServer();

        // CAMBIAR A LA ESCENA PRINCIPAL
        ServerChangeScene(mainScene);
    }

    // ============================
    // SERVIDOR PURO (Start Server)
    // ============================
    public override void OnStartServer()
    {
        base.OnStartServer();

        Debug.Log("Server iniciado (modo servidor dedicado). Anunciando servidor...");
        discovery.AdvertiseServer();
    }

    // ============================
    // CONEXIÓN DE CLIENTE DESCUBIERTO POR LAN
    // ============================
    public void ConnectToServer(ServerResponse info)
    {
        networkAddress = info.uri.Host;

        TelepathyTransport transport = Transport.active as TelepathyTransport;
        transport.port = (ushort)info.uri.Port;

        Debug.Log($"🔌 Conectando a {networkAddress}:{transport.port}");

        StartClient();
    }

    // ============================
    // CLIENTE CONECTADO
    // ============================
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("✅ CLIENTE conectado!");

        // El servidor ya cambió a Main_game. 
        // Mirror sincroniza la escena automáticamente.
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("❌ Cliente desconectado.");
    }
}
