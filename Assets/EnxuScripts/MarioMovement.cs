using UnityEngine;

public class MarioMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 0.4f;
    bool isFacingRight = false;
    float jumpPower = 1f;
    bool isJumping = false;

    Rigidbody2D rb;

    public GameObject groundCheck;
    public GroundCheckScript gcs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gcs = FindFirstObjectByType<GroundCheckScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets left/right input
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if(Input.GetButtonDown("Jump") && !isJumping && gcs.isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
            gcs.isGrounded = false;
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
}
