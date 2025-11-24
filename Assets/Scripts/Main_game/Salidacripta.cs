using UnityEngine;

public class SalidaCripta : MonoBehaviour
{
    public Transform puntoSpawnCementerio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = puntoSpawnCementerio.position;
        }
    }
}
