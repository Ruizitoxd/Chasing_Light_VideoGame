using UnityEngine;
using Mirror;
using Mirror.Discovery;
using System;
using System.Net;

public class MyLanDiscovery :
    NetworkDiscoveryBase<ServerRequest, ServerResponse>
{
    public long serverId;
    public MyLanManager manager;

    void Awake()
    {
        serverId = RandomLong();
        manager = (MyLanManager)NetworkManager.singleton;
    }
    void Start()
    {
        Debug.Log("MyLanDiscovery INICIADO");
    }

    // CLIENTE → Servidor
    protected override ServerRequest GetRequest()
    {
        return new ServerRequest();
    }

    // Servidor → CLIENTE
    protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {
        TelepathyTransport transport = Transport.active as TelepathyTransport;

        return new ServerResponse
        {
            serverId = serverId,
            uri = new Uri($"tcp4://{endpoint.Address}:{transport.port}"),
            name = manager.nombreServidor,
            players = NetworkServer.connections.Count,
            maxPlayers = manager.maxConnections,
            isOpen = NetworkServer.active
        };
    }


    // CLIENTE recibe un servidor
// CLIENTE recibe un servidor
    protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
    {
        Debug.Log("📡 Servidor recibido: " + response.name);

        var ui = FindAnyObjectByType<ServerListUI>();
        if (ui != null)
        {
            ui.AgregarServidor(response);
        }
    }


}
