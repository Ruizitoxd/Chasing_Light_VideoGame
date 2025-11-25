using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class PanelCrearServidorUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_InputField inputNombreServidor;
    public Button botonCrear;
    public Button botonCancelar;

    void Start()
    {
        botonCrear.onClick.AddListener(CrearServidor);
        botonCancelar.onClick.AddListener(() => gameObject.SetActive(false));
    }

    void CrearServidor()
    {
        Debug.Log("CREANDO HOST…");

        string nombre = inputNombreServidor.text;

        if (string.IsNullOrEmpty(nombre))
            nombre = "Servidor sin nombre";

        ((MyLanManager)NetworkManager.singleton).nombreServidor = nombre;

        NetworkManager.singleton.StartHost();
        Debug.Log("HOST INICIADO");

        ((MyLanManager)NetworkManager.singleton).discovery.AdvertiseServer();
        Debug.Log("DISCOVERY ANUNCIADO");

        gameObject.SetActive(false);
    }

}
