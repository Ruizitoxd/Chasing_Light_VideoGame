using System.Collections;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject hookPrefab;      // Prefab del gancho
    public float hookSpeed = 15f;      // Velocidad del gancho
    public float pullSpeed = 10f;      // Velocidad al jalar al jugador
    public Transform firePoint;        // Desde dónde sale el gancho

    private GameObject currentHook;
    private Vector3 hookTarget;
    private bool isPulling = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Click derecho
        {
            // Si ya hay un gancho, cancelarlo
            if (currentHook != null)
            {
                CancelHook();
            }
            else
            {
                FireHook();
            }
        }

        if (isPulling)
        {
            // Mover al jugador hacia el punto
            transform.position = Vector2.MoveTowards(transform.position, hookTarget, pullSpeed * Time.deltaTime);

            // Si llegamos cerca del punto del gancho -> parar
            if (Vector2.Distance(transform.position, hookTarget) < 0.3f)
            {
                CancelHook();
            }
        }
    }

    void FireHook()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        hookTarget = mousePos;

        currentHook = Instantiate(hookPrefab, firePoint.position, Quaternion.identity);

        // Mover el gancho hacia el mouse
        StartCoroutine(MoveHook());
    }

    IEnumerator MoveHook()
    {
        while (currentHook != null && Vector2.Distance(currentHook.transform.position, hookTarget) > 0.1f)
        {
            currentHook.transform.position = Vector2.MoveTowards(currentHook.transform.position, hookTarget, hookSpeed * Time.deltaTime);
            yield return null;
        }

        // Cuando llega al objetivo -> empieza a jalar al jugador
        if (currentHook != null)
        {
            isPulling = true;
        }
    }

    void CancelHook()
    {
        isPulling = false;

        if (currentHook != null)
        {
            Destroy(currentHook);
            currentHook = null;
        }
    }
}
