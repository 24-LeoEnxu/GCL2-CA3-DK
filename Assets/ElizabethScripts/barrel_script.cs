using UnityEngine;

public class barrel_script : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 0.5f; // constant horizontal roll speed, never changes once set

    [SerializeField] private bool rotateWithVelocity = true; // spins the sprite to look like its rolling
    [SerializeField] private float rotationMultiplier = -500f;

    private Rigidbody2D rb; // needed for gravity/falling only now
    private float currentDirection = 0f; // 0 = not moving yet, 1 = right, -1 = left

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // grab the rigidbody once
    }

    void Update()
    {
        if (currentDirection != 0f)
        {
            // move horizontally at a fixed constant speed, ignoring physics/gravity influence on X
            transform.position += new Vector3(rollSpeed * currentDirection * Time.deltaTime, 0f, 0f);
        }

        if (rotateWithVelocity)
        {
            transform.Rotate(0f, 0f, rollSpeed * currentDirection * rotationMultiplier * Time.deltaTime); // visual spin based on constant speed
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlatformDirection platformDir = other.GetComponent<PlatformDirection>(); // check if this has a direction script on it
        if (platformDir != null)
        {
            currentDirection = platformDir.rollRight ? 1f : -1f; // lock in constant direction
        }
        else if (other.CompareTag("Player"))
        {
            LevelManagerScript.Instance.playerDeath(); // tell the level manager mario got hit
        }
    }
}