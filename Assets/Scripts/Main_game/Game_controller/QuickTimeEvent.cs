using Mirror;
using UnityEngine;

public class QuickTimeEvent : NetworkBehaviour
{
    [SyncVar] public bool eventActive = false;

    private NetworkIdentity killer;
    private NetworkIdentity victim;

    private int killerPoints = 0;
    private int victimPoints = 0;

    public float duration = 3f; 
    private float timeLeft;

    public void StartQTE(NetworkIdentity killer, NetworkIdentity victim)
    {
        if (!isServer) return;

        this.killer = killer;
        this.victim = victim;

        killerPoints = 0;
        victimPoints = 0;

        timeLeft = duration;
        eventActive = true;

        // Lanzar QTE en pantallas
        RpcStartQTE(killer.netId, victim.netId);
    }

    [ClientRpc]
    void RpcStartQTE(uint killerID, uint victimID)
    {
        NetworkIdentity k = NetworkClient.spawned[killerID];
        NetworkIdentity v = NetworkClient.spawned[victimID];

        if (k.isLocalPlayer)
            QTE_UI.Instance.StartKillerQTE();

        if (v.isLocalPlayer)
            QTE_UI.Instance.StartVictimQTE();
    }

    [Command]
    public void CmdAddKillerPoint()
    {
        if (!eventActive) return;
        killerPoints++;
    }

    [Command]
    public void CmdAddVictimPoint()
    {
        if (!eventActive) return;
        victimPoints++;
    }

    void Update()
    {
        if (!isServer || !eventActive) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            eventActive = false;

            if (killerPoints > victimPoints)
                RpcKillerWins();
            else
                RpcVictimWins();
        }
    }

    [ClientRpc]
    void RpcKillerWins()
    {
        if (victim != null)
        {
            // Matar al superviviente
            victim.GetComponent<PlayerHealth>().Kill();
        }

        QTE_UI.Instance.EndQTE();
    }

    [ClientRpc]
    void RpcVictimWins()
    {
        if (victim != null)
        {
            // Liberar al superviviente
            victim.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        QTE_UI.Instance.EndQTE();
    }
    public static QuickTimeEvent localQTE;

        public override void OnStartLocalPlayer()
        {
            localQTE = this;
        }

}
