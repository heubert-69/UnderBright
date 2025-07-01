using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    private bool isGrounded;

    void Update()
    {
        Vector2 velocity = myRigidbody.velocity;


        if (Keyboard.current.aKey.isPressed)
        {
            velocity.x = -2f ;
        }

        else if (Keyboard.current.dKey.isPressed)
        {
            velocity.x = 2f;
        }
        else
        {
            velocity.x = 0f; 
        }


        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = 5f; 
            isGrounded = false;
        }

        myRigidbody.velocity = velocity * 10;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
