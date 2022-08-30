using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Animator anim;

    private float groundCheckRadius = 0.3f;
    [SerializeField] private bool isGrounded = false;
    private bool facingRight = true;
    private int jumpMax = 2;
    private int jumpsAvailable = 0;

    private float gravity;
    private float jumpHeight = 2.0f;
    private float jumpTime = 0.75f;
    private float initialJumpVelocity;

    private float horizontalInput;
    private float moveSpeed = 450.0f;

    private void Start() {
        float timeToApex = jumpTime / 2.0f;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        
        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);

        rb.gravityScale = gravity / Physics2D.gravity.y;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(groundCheckPoint.position, groundCheckRadius);    
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if((!facingRight && horizontalInput > 0.01f) ||
        (facingRight && horizontalInput < -0.01f)) 
        {
            Flip();
        }

        if(Input.GetButtonDown("Jump") && jumpsAvailable > 1)
        {
            Jump();
        }

        if(isGrounded)
        {
            jumpsAvailable = jumpMax;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate() {
        float xVelocity = horizontalInput * moveSpeed * Time.deltaTime;
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);

        if(rb.velocity.x > 0.01f || rb.velocity.x < -0.01f) {
            anim.SetBool("isRunning", true);
        } else {
            anim.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, initialJumpVelocity);
        if(jumpsAvailable != 0)
        {
            anim.SetTrigger("Jump");
        }
        jumpsAvailable--;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180);
    }
}
