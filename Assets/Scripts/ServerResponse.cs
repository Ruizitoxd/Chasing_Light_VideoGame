using Mirror;
using System;
using Mirror.Discovery;

public struct ServerResponse : NetworkMessage
{
    public long serverId;
    public Uri uri;

    public string name;
    public int players;
    public int maxPlayers;
    public bool isOpen;
}
