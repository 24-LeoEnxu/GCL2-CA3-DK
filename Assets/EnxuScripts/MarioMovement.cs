using UnityEngine;

public class MarioMovement : MonoBehaviour
{
    //Animation
    Animator animator;

    //WASD or Arrow Keys Inputs, move and jump speed
    float horizontalInput;
    float moveSpeed = 0.4f;
    bool isFacingRight = false;
    float jumpPower = 1f;
    bool isJumping = false;

    Rigidbody2D rb;

    //GroundCheck
    public GameObject groundCheck;
    public GroundCheckScript gcs;

    //Ladder
    bool isClimbing = false;
    BoxCollider2D bCollider;
    float climbSpeed = 0.15f;

    //Hammer
    bool hammerPower = false;
    public GameObject hammerHitbox;
    float hammerTimer = 8f;

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
        if(Input.GetKey(KeyCode.W) && isClimbing || Input.GetKey(KeyCode.UpArrow) && isClimbing)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + climbSpeed * Time.deltaTime);
        }

        //If player is on ladder and press S/Down
        if (Input.GetKey(KeyCode.S) && isClimbing || Input.GetKey(KeyCode.DownArrow) && isClimbing)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - climbSpeed * Time.deltaTime);
        }

        //Animation change for hammer powerup pickup
        if(hammerPower)
        {
            isClimbing = false;
            animator.SetBool("MarioHammer", true);
            hammerTimer -= Time.deltaTime;
        }

        if(hammerTimer <= 0)
        {
            hammerPower = false;
            animator.SetBool("MarioHammer", false);
            hammerTimer = 8f;
            hammerHitbox.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y); //Moves player
    }

    void FlipSprite()
    {
        //Flips sprite depending on player input
        if(isFacingRight && horizontalInput < 0f && !isClimbing|| !isFacingRight && horizontalInput > 0f && !isClimbing)
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Check if player is on ladder
        if(collider.CompareTag("Ladder"))
        {
            isClimbing = true;
            bCollider.isTrigger = true;
            rb.gravityScale = 0f;
            isJumping = true;
        }

        if(collider.CompareTag("HammerPowerup"))
        {
            Destroy(collider.gameObject);
            hammerPower = true;
            hammerHitbox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        //Check if player is off ladder
        if(collider.CompareTag("Ladder"))
        {
            isClimbing = false;
            bCollider.isTrigger = false;
            rb.gravityScale = 0.2f;
            isJumping = false;
        }
    }
}
