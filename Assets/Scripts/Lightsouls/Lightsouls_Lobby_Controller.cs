using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsouls_Lobby_Controller : MonoBehaviour
{
    //Velocidad
    public float WalkSpeed = 5f;
    public float RunSpeed = 9f;
    //Altura salto
    public float JumpForce;

    public LayerMask GroundLayer;

    public GameObject raycast;

    //Definir fisicas dentro del script
    private Rigidbody2D Rigidbody2D;

    //Variable para definr el movimiento del personaje
    private float Horizontal;

    private bool Grounded;

    private Animator Animator;

    private bool isRunning;

    //Variable para guardar la ultima direccion
    private int LastDirection = 1;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { //Input del teclado
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Detectar si corre (Shift + dirección)
        isRunning = Input.GetKey(KeyCode.LeftShift) && Horizontal != 0;



        // Direccion de vista del jugador
        if (Horizontal > 0)
        {
            LastDirection = 1;
        }
        else if (Horizontal < 0)
        {
            LastDirection = -1;
        }

        // Aplicar la direccion guardada
        transform.localScale = new Vector3(LastDirection, 1, 1);


        Animator.SetBool("Walk", Horizontal != 0.0f);

        Animator.SetBool("Run", isRunning);

        Animator.SetBool("Grounded", Grounded);

        Animator.SetFloat("YVelocity", Rigidbody2D.velocity.y);


        Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(raycast.transform.position, Vector3.down, 0.1f, GroundLayer))
        {
            Grounded= true;
        }
        else
        {
            Grounded= false;
        }
        

        //Salto
        if (Input.GetKeyDown(KeyCode.W) && Grounded==true)
        {
            Jump();
        }
    }

    //Funcion Salto
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce,ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        float currentSpeed = isRunning ? RunSpeed : WalkSpeed;
        Rigidbody2D.velocity = new Vector2(Horizontal * currentSpeed, Rigidbody2D.velocity.y);

    }

}
