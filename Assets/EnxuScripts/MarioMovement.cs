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
    bool isClimbing = false;
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
        if (Input.GetButtonDown("Jump") && !isJumping && gcs.isGrounded && !hammerPower && !isClimbing)
        {
            LevelManagerScript.Instance.play_jumpSFX();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
            gcs.isGrounded = false;
        }

        // animation float
        animator.SetFloat("MarioMoving", horizontalInput);

        // ----------------------- LADDER ----------------------- //

        // if player is on ladder
        if (isClimbing)
        {
            animator.SetBool("MarioClimb", true);
        }
        if (!isClimbing)
        {
            animator.SetBool("MarioClimb", false);
        }
    }

    // ----------------------- END OF VOID UPDATE() ----------------------- //

    // changed to OnTriggerStay2D instead of OnTriggerEnter2D to stop immediate climbing
    private void OnTriggerStay2D(Collider2D collider)
    {
        // check if player is on ladder
        if (collider.CompareTag("Ladder"))
        {
            isClimbing = true;
            bCollider.isTrigger = true;
            rb.gravityScale = 0f;
            isJumping = true;

            // get W key to move
            if (Input.GetKey(KeyCode.W) && isClimbing || Input.GetKey(KeyCode.UpArrow) && isClimbing)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + climbSpeed * Time.deltaTime);
            }

            // get S key to move
            if (Input.GetKey(KeyCode.S) && isClimbing || Input.GetKey(KeyCode.DownArrow) && isClimbing)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - climbSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // check if player is off ladder
        if (collider.CompareTag("Ladder"))
        {
            isClimbing = false;
            bCollider.isTrigger = false;
            rb.gravityScale = 0.2f;
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("HammerPowerup"))
        {
            Destroy(collider.gameObject);
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

    private void FixedUpdate()
    {
        if (!canRun)
            return;

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        bool isMoving = horizontalInput != 0;

        if (isMoving)
        {
            LevelManagerScript.Instance.play_marioWalkSFX();
        }
        else
        {
            LevelManagerScript.Instance.stop_marioWalkSFX();
        }
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
