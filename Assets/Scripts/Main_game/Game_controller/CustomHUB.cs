using Mirror;
using UnityEngine;

public class CustomHUD : MonoBehaviour
{
    void OnGUI()
    {
        // Solo mostrar si el servidor está activo
        if (!NetworkServer.active || !NetworkClient.isConnected)
            return;

        GUILayout.BeginArea(new Rect(10, 300, 250, 120));
        GUILayout.BeginVertical("box");

        GUILayout.Label("Custom Controls (Host)");

        if (GUILayout.Button("INICIAR PARTIDA (Roles Random)"))
        {
            MyNetworkManager nm = NetworkManager.singleton as MyNetworkManager;
            if (nm != null)
            {
                nm.IntentarIniciarYAsignarRoles();
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
