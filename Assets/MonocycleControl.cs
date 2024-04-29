using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonocycleControl : MonoBehaviour
{
    // Variables to control clown movement and balance
    public float movementSpeed = 5f; // Speed of movement
    public float maxJumpForce = 20f, minJumpForce = 2f; // Maximum force applied when holding jump key
    public float maxJumpTime = 2f; // Maximum time the jump key can be held
    public float maxTiltAngle = 30f; // Maximum tilt angle for balancing
    public float tiltSpeed = 5f; // Speed of tilting

    public Vector3 groundCheckOffset = Vector3.down;
    public float groundCheckRadius = 0.5f;
    public Slider jumpMeter;

    private Animator anim;
    private Rigidbody2D rb; // Reference to the clown's Rigidbody component
    private bool isGrounded; // Flag to track if the clown is grounded
    public LayerMask groundLayer;
    private float horizontalInput;
    private float jumpTime; // Time the jump key has been held

    void Start()
    {
        // Get the Rigidbody component attached to the clown
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = Vector3.zero;
        anim = GetComponent<Animator>();
        jumpMeter.maxValue = 1;
    }

    void Update()
    {
        // Handle player input for movement
        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("speed", rb.velocity.x);

        jumpMeter.value=Mathf.Lerp(0f, 1f, jumpTime / maxJumpTime);
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTime = 0f;
            jumpMeter.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && jumpTime < maxJumpTime)
        {
            jumpTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            jumpMeter.gameObject.SetActive(false);
            Jump();
            anim.ResetTrigger("Land");
            anim.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position+Vector3.down, 0.5f, groundLayer);
        if (isGrounded)
        {
            anim.SetTrigger("Land");
        }

        Vector2 movement = new Vector2(horizontalInput * movementSpeed, 0);

        if (horizontalInput != 0)
        {
            anim.SetFloat("Orientation", horizontalInput);
        }

        float tiltAmount = 0f;
        if (isGrounded)
        {
            rb.centerOfMass = Vector3.zero;
            rb.AddForce(movement, ForceMode2D.Force);
            tiltAmount = rb.rotation + 45 * -horizontalInput;
        }
        else
        {
            rb.centerOfMass = new Vector3(0,1,0);
            // No tilt restriction when not grounded
            tiltAmount = rb.rotation + 45 * -horizontalInput;
        }

        // Ensure clown stays balanced within maxTiltAngle
        Quaternion targetRotation = Quaternion.Euler(0, 0, tiltAmount);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed));
    }

    void Jump()
    {
        // Calculate jump force based on how long the key was held
        float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpTime / maxJumpTime);
        // Apply upward force to simulate jumping
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }
}
