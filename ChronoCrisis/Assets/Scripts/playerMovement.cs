using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float hitPoints = 100f;
    public float movementSpeed = 5f;
    public float RunMulti = 2f;

    private bool isRunning = false;
    private bool isAttacking = false;
    private bool isHealing = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            Debug.LogError("The RigidBody2D is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    // Player movement
    void movement()
    {
        float verticalInput =   Input.GetKey(KeyCode.W) ? 1f : 
                                Input.GetKey(KeyCode.S) ? -1f : 0f;

        float horizontalInput = Input.GetKey(KeyCode.D) ? 1f :
                                Input.GetKey(KeyCode.A) ? -1f : 0f;

        Vector2 directionMove =  new Vector2(horizontalInput, verticalInput);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = directionMove * movementSpeed * RunMulti;
        }
        else
        {
            rb.velocity = directionMove * movementSpeed;
        }
    }

    //basic attack
    void attack(){

    }

    //skill casting
    void skillCastQ(){

    }

    void skillCastE(){

    }

    void skillCastR(){

    }

    void skillCastT(){

    }

    //interact (F)
    void interact(){

    }

    //recieved item
    void recievedPowerUp(){
        
    }
}
