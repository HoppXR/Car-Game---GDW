using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    Rigidbody2D body;

    [SerializeField] float accelerationPower;
    [SerializeField] float steeringPower;
    [SerializeField] float boostForce;
    [SerializeField] float driftPower;
    [SerializeField] float maxSpeedOnRoad;
    [SerializeField] float maxSpeedOffRoad;
    [SerializeField] float MaxSpeedWhileDrifting;
    [SerializeField] float maxSpeedWithBoost;


    float steeringInput;
    float horizontalInput;
    float maxSpeed;
    float accelerationInput;
    float rotationAngle;
    float velocityVsUp;
    float driftBoostTimer;

    bool canDrive;
    bool offRoad;
    bool isDrifting;
    bool driftActivated;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        // TEMP (change later)
        canDrive = true;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        accelerationInput = Input.GetAxis("Vertical");

        if (offRoad)
        {
            AdjustSpeedForOffroad();
        }
        else
        {
            maxSpeed = maxSpeedOnRoad;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDrifting = true;
            maxSpeed = MaxSpeedWhileDrifting;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isDrifting = false;

            if (offRoad)
            {
                maxSpeed = maxSpeedOffRoad;
            }
            else
            {
                maxSpeed = maxSpeedOnRoad;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canDrive)
        {
            ApplySteering();

            ApplyEngineForce();

            KillOrthogonalVelocity();

            if (velocityVsUp > maxSpeed)
            {
                SlowDownToMaxSpeed();
            }

            if (isDrifting && !driftActivated && steeringInput != 0)
            {
                ActivateDrift();
            }
            else if ((!isDrifting && driftActivated) || steeringInput == 0)
            {
                DeactivateDrift();
            }

            if (driftActivated)
            {

            }
            else
            {

            }
        }
        else
        {
            body.velocity = new Vector2(0,0);
        }
    }

    public void ApplySpeedBoost()
    {
        body.AddForce(transform.up * boostForce, ForceMode2D.Impulse);
    }

    void ActivateDrift()
    {
        driftPower = 0.9f;
        steeringPower = 2.0f;
        driftActivated = true;
    }

    void DeactivateDrift()
    {
        driftPower = 0.5f;
        steeringPower = 0.5f;
        driftActivated = false;
    }

    void AdjustSpeedForOffroad()
    {
        maxSpeed = maxSpeedOffRoad;
    }

    void ApplySteering()
    {
        if (horizontalInput < 0 && steeringInput > 0 && ((accelerationInput <= 0 && velocityVsUp > 0) || (accelerationInput >= 0 && velocityVsUp < 0)))
        {
            steeringInput = Mathf.Lerp(steeringInput, -1, Time.fixedDeltaTime);
        }
        else if (horizontalInput > 0 && steeringInput < 0 && ((accelerationInput <= 0 && velocityVsUp > 0) || (accelerationInput >= 0 && velocityVsUp < 0)))
        {
            steeringInput = Mathf.Lerp(steeringInput, 1, Time.fixedDeltaTime);
        }
        else
        {
            steeringInput = horizontalInput;
        }
        
        float minSpeedForTurn = ((body.velocity.magnitude) / 8);

        minSpeedForTurn = Mathf.Clamp01(minSpeedForTurn);
        
        if (accelerationInput <= 0 && velocityVsUp > 0 || accelerationInput >= 0 && velocityVsUp < 0)
        {
            steeringPower = Mathf.Lerp(steeringPower, 0.0f, Time.fixedDeltaTime * 2);
        }
        else if (driftActivated)
        {
            steeringPower = 2.5f;
        }
        else
        {
            steeringPower = 1.5f;
        }

        if (velocityVsUp <= 0)
        {
            rotationAngle += steeringInput * steeringPower * minSpeedForTurn;
        }
        else
        {
            rotationAngle -= steeringInput * steeringPower * minSpeedForTurn;
        }

        body.rotation = rotationAngle;
    }

    void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, body.velocity);

        if (velocityVsUp >= maxSpeed && accelerationInput > 0)
        {
            return;
        } 

        if (velocityVsUp <= -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        if (accelerationInput == 0 || (accelerationInput < 0 && velocityVsUp > 0))
        {
            body.drag = Mathf.Lerp(body.drag, 5.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            body.drag = 0;
        }

        Vector2 engineForce = transform.up * accelerationInput * accelerationPower;

        body.AddForce(engineForce, ForceMode2D.Force);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(body.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(body.velocity, transform.right);

        body.velocity = forwardVelocity + rightVelocity * driftPower;
    }

    void SlowDownToMaxSpeed()
    {
        Vector2 slowdownForce = -transform.up * (accelerationPower * 2.0f);

        body.AddForce(slowdownForce, ForceMode2D.Force);
    }
}
