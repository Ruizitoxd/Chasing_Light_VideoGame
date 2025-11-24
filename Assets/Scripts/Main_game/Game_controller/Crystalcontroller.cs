using Mirror;
using UnityEngine;

public class Crystalcontroller : NetworkBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo un jugador puede recogerlo
        if (!other.CompareTag("Player")) return;

        PlayerRole p = other.GetComponent<PlayerRole>();
        if (p == null) return;

        // El cliente debe pedir al servidor que sume el fragmento
        p.CmdRecogerFragmento(gameObject);
    }
}
