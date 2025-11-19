using UnityEngine;

public class Crystalcontroller : MonoBehaviour
{
    private Partida_controller partida;

    void Start()
    {
        partida = FindObjectOfType<Partida_controller>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (partida != null)
        {
            partida.SumarFragmentos();
        }

        Destroy(gameObject);
    }
}
