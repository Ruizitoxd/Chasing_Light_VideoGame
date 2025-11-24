using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaSensorController : MonoBehaviour
{
    [SerializeField] public GameObject sensor1;
    [SerializeField] public GameObject sensor2;

    private SpriteRenderer sprite;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        bool activo1 = sensor1.GetComponent<SensorController>().sensorIsActive;
        bool activo2 = sensor2.GetComponent<SensorController>().sensorIsActive;

        if (activo1 && activo2)
        {
            // Desactiva visual y colisión
            sprite.enabled = false;
            boxCol.enabled = false;
        }
        else
        {
            // Reactiva visual y colisión
            sprite.enabled = true;
            boxCol.enabled = true;
        }
    }
}
