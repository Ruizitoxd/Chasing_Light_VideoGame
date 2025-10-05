using UnityEngine;

public class SalidaCripta : MonoBehaviour
{
    public GameObject cementerio;
    public GameObject cripta;
    public Transform puntoSpawnCementerio;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cripta.SetActive(false);
            cementerio.SetActive(true);
            other.transform.position = puntoSpawnCementerio.position;
        }
    }
}
