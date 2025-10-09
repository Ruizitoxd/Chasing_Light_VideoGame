using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : NetworkBehaviour
    {
        public float speed;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }


    private void Update()
    {
        //Evitar movimiento si no es el jugador actual
        if (!isLocalPlayer) return;

        Vector2 dir = Vector2.zero;

        // Movimiento en X
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        // Movimiento en Y
        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }

        // Normalizar para que diagonales no sean más rápidas
        dir.Normalize();

        // Configurar animaciones
        if (dir.magnitude > 0)
        {
            animator.SetBool("IsMoving", true);

            if (dir.y > 0) animator.SetInteger("Direction", 1);   // Arriba
            else if (dir.y < 0) animator.SetInteger("Direction", 0); // Abajo
            else if (dir.x > 0) animator.SetInteger("Direction", 2); // Derecha
            else if (dir.x < 0) animator.SetInteger("Direction", 3); // Izquierda
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        // Mover Rigidbody
        GetComponent<Rigidbody2D>().velocity = speed * dir;
    }

    }
}
