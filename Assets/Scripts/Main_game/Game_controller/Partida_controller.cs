using TMPro;
using UnityEngine;

public class Partida_controller : MonoBehaviour
{
    [Header("Fragmentos")]
    public int CantidadFragmentos = 0;
    public int FragmentosNecesarios = 5;

    [Header("Tiempo")]
    public float TiempoJugado = 0f;
    public TMP_Text tiempoTXT;

    [Header("Portal")]
    public GameObject Portal;

    void Update()
    {
        ActualizarTiempo();
    }

    public void ActualizarTiempo()
    {
        TiempoJugado += Time.deltaTime;

        // Convertir a formato mm:ss
        int minutos = Mathf.FloorToInt(TiempoJugado / 60f);
        int segundos = Mathf.FloorToInt(TiempoJugado % 60f);

        tiempoTXT.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public int SumarFragmentos()
    {
        CantidadFragmentos++;

        if (CantidadFragmentos >= FragmentosNecesarios)
            ActivarPortal();

        return CantidadFragmentos;
    }

    public bool ActivarPortal()
    {
        if (Portal != null)
        {
            Portal.SetActive(true);
            return true;
        }
        return false;
    }
}
