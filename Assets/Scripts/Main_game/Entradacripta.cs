using UnityEngine;

public class EntradaCripta : MonoBehaviour
{
    public GameObject cementerio;
    public GameObject cripta;
    public Transform puntoSpawnCripta;
    public Transform puntoSpawnCementerio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Desactivar cementerio
            cementerio.SetActive(false);

            // Activar cripta
            cripta.SetActive(true);

            // Mover jugador al punto de spawn en la cripta
            other.transform.position = puntoSpawnCripta.position;
        }
    }
}
