using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    Rigidbody2D body;

    [SerializeField] float accelerationPower;
    [SerializeField] float steeringPower;

    float steeringInput;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        steeringInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            body.AddForce(accelerationPower * -transform.up, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.AddForce(accelerationPower * transform.up, ForceMode2D.Force);
        }

        body.MoveRotation(body.rotation + (-steeringInput * steeringPower));
    }
}
