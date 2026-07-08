using UnityEngine;

public class MarioMovement : MonoBehaviour
{
    Animator animator;

    float horizontalInput;
    float moveSpeed = 0.4f;
    bool isFacingRight = false;
    float jumpPower = 1f;
    bool isJumping = false;

    Rigidbody2D rb;

    public GameObject groundCheck;
    public GroundCheckScript gcs;

    bool isClimbing = false;
    BoxCollider2D bCollider;
    float climbForce = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gcs = FindFirstObjectByType<GroundCheckScript>();
        animator = GetComponent<Animator>();
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets left/right input
        horizontalInput = Input.GetAxis("Horizontal");

        //Flips Sprite depending on where player moves
        FlipSprite();

        //Jump script
        if(Input.GetButtonDown("Jump") && !isJumping && gcs.isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
            gcs.isGrounded = false;
        }

        //Animation float
        animator.SetFloat("MarioMoving", horizontalInput);

        //If player is on ladder
        if(isClimbing)
        {
            animator.SetBool("MarioClimb", true);
        }
        if(!isClimbing)
        {
            animator.SetBool("MarioClimb", false);
        }

        //If player is on ladder and press W/Up
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && isClimbing)
        {
            rb.AddForce(transform.up * climbForce);
        }

        //If player is on ladder and press S/Down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && isClimbing)
        {
            rb.AddForce(transform.up * -climbForce);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y); //Moves player
    }

    void FlipSprite()
    {
        //Flips sprite depending on player input
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }

    //Check if player is on ladder
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Ladder"))
        {
            isClimbing = true;
            bCollider.isTrigger = true;
            rb.gravityScale = 0f;
            isJumping = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Ladder"))
        {
            isClimbing = false;
            bCollider.isTrigger = false;
            rb.gravityScale = 0.2f;
            isJumping = false;
        }
    }
}
