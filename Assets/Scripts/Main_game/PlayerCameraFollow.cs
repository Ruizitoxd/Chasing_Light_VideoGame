using UnityEngine;
using Mirror;
using Cainos.PixelArtTopDown_Basic;

public class PlayerCameraFollow : NetworkBehaviour
{
    public CameraFollow cameraPrefab; // Prefab de la cámara que sigue al jugador

    private CameraFollow cameraInstance;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Solo instanciamos la cámara para el jugador local
        if (cameraPrefab != null)
        {
            cameraInstance = Instantiate(cameraPrefab);
            cameraInstance.target = this.transform; // La cámara sigue a este jugador
        }
    }
}
