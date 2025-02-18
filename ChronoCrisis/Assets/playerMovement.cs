using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //player mechanic
    //movement
    void movement(){
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A)) rb.AddForce(Vector3.left);
        if (Input.GetKey(KeyCode.D)) rb.AddForce(Vector3.right);
        if (Input.GetKey(KeyCode.W)) rb.AddForce(Vector3.forward);
        if (Input.GetKey(KeyCode.S)) rb.AddForce(Vector3.back);

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

    // Update is called once per frame
    void Update()
    {
        movement();

    }
}
