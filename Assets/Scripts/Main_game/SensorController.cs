using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    public bool sensorIsActive = false;

    private void OnTriggerEnter2D(Collider2D other) {
        sensorIsActive = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        sensorIsActive = false;
    }
}
