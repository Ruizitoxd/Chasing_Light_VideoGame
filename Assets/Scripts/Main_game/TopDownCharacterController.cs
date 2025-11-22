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

        // Sistema de partículas
        public ParticleSystem ps;

        private void Start()
        {
            animator = GetComponent<Animator>();

            ps.Stop(); // Aseguramos que inicie apagado
        }

        private void Update()
        {
            //Evitar movimiento si no es el jugador actual
            if (!isLocalPlayer) return;

            Vector2 dir =  new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            bool isMoving = dir.sqrMagnitude > 0.01f;

            // 🔥 Encender / apagar partículas según movimiento
            if (isMoving && !ps.isPlaying)
                ps.Play();
            else if (!isMoving && ps.isPlaying)
                ps.Stop();

            // Escalar partículas según velocidad
            ps.transform.localScale =
                Vector3.MoveTowards(ps.transform.localScale, Vector3.one * dir.magnitude, 5 * Time.deltaTime);

            // Dirección para animaciones
            HandleAnimationDirection(dir);

            // Actualiza dirección del humo
            UpdateParticlesDirection(dir);

            // Normalizar para que diagonales no sean más rápidas
            dir.Normalize();

            // Configurar animaciones
            animator.SetBool("IsMoving", isMoving);

            // Mover Rigidbody
            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }

        private void HandleAnimationDirection(Vector2 dir)
        {
            if (dir.x < 0) animator.SetInteger("Direction", 3);
            if (dir.x > 0) animator.SetInteger("Direction", 2);
            if (dir.y > 0) animator.SetInteger("Direction", 1);
            if (dir.y < 0) animator.SetInteger("Direction", 0);
        }

        // Direcciona las particulas en la dirección opuesta al movimiento
        void UpdateParticlesDirection(Vector2 moveDir)
        {
            if (moveDir.sqrMagnitude < 0.01f) return;

            var velocity = ps.velocityOverLifetime;
            velocity.enabled = true;

            Vector2 opposite = -moveDir.normalized;

            velocity.x = opposite.x * 3f;
            velocity.y = opposite.y * 3f;
        }

    }
}
