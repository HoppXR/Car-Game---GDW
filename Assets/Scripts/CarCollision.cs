using UnityEngine;

public class CarCollision : MonoBehaviour
{
    Rigidbody2D body;

    CarMovement movement;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        movement = GetComponent<CarMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BoostPad")
        {
            movement.ApplySpeedBoost();
        }
    }
}
