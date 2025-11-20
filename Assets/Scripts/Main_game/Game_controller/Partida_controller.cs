using Mirror;
using TMPro;
using UnityEngine;

public class Partida_controller : NetworkBehaviour
{
    [Header("Fragmentos")]
    public int CantidadFragmentos = 0;
    public int FragmentosNecesarios = 5;

    [Header("Tiempo")]
    [SyncVar(hook = nameof(OnTiempoChanged))]
    public float TiempoJugado = 0f;
    public TMP_Text tiempoTXT;

    [Header("Portal")]
    public GameObject Portal;

    void Update()
    {
        if (isServer)
        {
            // Solo el servidor aumenta el tiempo
            TiempoJugado += Time.deltaTime;
        }
    }

    void OnTiempoChanged(float oldValue, float newValue)
    {
        // Actualiza el texto en cada cliente
        if (tiempoTXT != null)
        {
            int minutos = Mathf.FloorToInt(newValue / 60f);
            int segundos = Mathf.FloorToInt(newValue % 60f);
            tiempoTXT.text = minutos.ToString("00") + ":" + segundos.ToString("00");
        }
    }

    // Fragmentos
    public void SumarFragmentos()
    {
        CantidadFragmentos++;

        if (CantidadFragmentos >= FragmentosNecesarios)
            ActivarPortal();
    }

    public void ActivarPortal()
    {
        if (Portal != null)
        {
            Portal.SetActive(true);
        }
    }
}
