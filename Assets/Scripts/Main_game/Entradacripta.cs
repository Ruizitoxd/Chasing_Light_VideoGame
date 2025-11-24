using UnityEngine;

public class EntradaCripta : MonoBehaviour
{
    public Transform puntoSpawnCripta;
    public Transform puntoSpawnCementerio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mover jugador al punto de spawn en la cripta
            other.transform.position = puntoSpawnCripta.position;
        }
    }
}
