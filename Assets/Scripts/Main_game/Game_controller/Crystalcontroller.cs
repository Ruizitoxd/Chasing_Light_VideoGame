using UnityEngine;

public class Crystalcontroller : MonoBehaviour
{
    private Partida_controller partida;

    public string[] animationStates; // Nombres de los estados en tu Animator
    private Animator anim;

    void Start()
    {
        partida = FindObjectOfType<Partida_controller>();
        anim = GetComponent<Animator>();

        // Elegir un estado al azar
        if (animationStates.Length > 0)
        {
            string randomState = animationStates[Random.Range(0, animationStates.Length)];
            anim.Play(randomState);
        }
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
