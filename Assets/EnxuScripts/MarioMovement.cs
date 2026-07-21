using System.Collections;
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
    private bool isOnLadder = false;
    private bool isClimbing = false;

    private Transform currentLadder;

    BoxCollider2D bCollider;
    float climbSpeed = 0.15f;

    //Hammer
    bool hammerPower = false;
    public GameObject hammerHitbox;
    float hammerTimer = 8f;

    //3s wait boolean
    private bool canRun = false;

    // Changed to IEnumerator to fit the 3s wait criteria
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        canRun = true;

        rb = GetComponent<Rigidbody2D>();
        gcs = FindFirstObjectByType<GroundCheckScript>();
        animator = GetComponent<Animator>();
        bCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canRun)
            return;

        // gets left/right input
        horizontalInput = Input.GetAxis("Horizontal");

        // flips Sprite depending on where player moves
        FlipSprite();

        // jump script
        if (Input.GetButtonDown("Jump") && !isJumping && gcs.isGrounded && !isClimbing)
        {
            LevelManagerScript.Instance.play_jumpSFX();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
            gcs.isGrounded = false;
        }

        // animation float
        animator.SetFloat("MarioMoving", horizontalInput);

        // ----------------------- LADDER ----------------------- //

        // check if mario is actually staying on the ladder
        if (isOnLadder && !isClimbing)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.position = new Vector2(currentLadder.position.x,transform.position.y);

                isClimbing = true;
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                bCollider.isTrigger = true;
            }
        }

        if (isClimbing)
        {
            float vertical = Input.GetAxisRaw("Vertical");

            rb.linearVelocity = new Vector2(0f, vertical * climbSpeed);

            animator.SetBool("MarioClimb", vertical != 0);
        }
    }

    // ----------------------- END OF VOID UPDATE() ----------------------- //

    private void OnTriggerEnter2D(Collider2D other)
    {
        // checks if mario is colliding with ladder
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = true;
            currentLadder = other.transform;
        }

        // ----------------------- hammer power up ----------------------- //
        if (other.CompareTag("HammerPowerup"))
        {
            Destroy(other.gameObject);

            hammerPower = true;
            hammerHitbox.SetActive(true);

            LevelManagerScript.Instance.play_hammerTimeSFX();
        }

        // animation change for hammer powerup pickup
        if (hammerPower)
        {
            isClimbing = false;
            isJumping = false;

            animator.SetBool("MarioHammer", true);
            hammerTimer -= Time.deltaTime;
        }

        if (hammerTimer <= 0)
        {
            hammerPower = false;
            animator.SetBool("MarioHammer", false);
            hammerTimer = 7f;
            hammerHitbox.SetActive(false);

            LevelManagerScript.Instance.stop_HammerTimeSFX();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;

            currentLadder = null;

            rb.gravityScale = 0.2f;
            rb.linearVelocity = Vector2.zero;
            bCollider.isTrigger = false;

            animator.SetBool("MarioClimb", false);
        }
    }

    private void FixedUpdate()
    {
        if (!canRun)
            return;

        if (isClimbing)
            return;

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

       // bool isMoving = horizontalInput != 0;
       // ^ commented to test if it's redundant, leaving it here in case it's important
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
}
