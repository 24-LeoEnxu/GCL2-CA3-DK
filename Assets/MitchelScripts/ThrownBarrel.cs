using UnityEngine;

/// <summary>
/// Attach to the aimed-throw barrel prefab.
/// Requires a CircleCollider2D set to Is Trigger. No Rigidbody2D needed —
/// movement is done manually via transform so it ignores platform collisions entirely.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class ThrownBarrel : MonoBehaviour
{
    [Tooltip("How long before this barrel auto-destroys if it never hits anything (safety cleanup).")]
    [SerializeField] private float lifetime = 5f;

    [Tooltip("Player tag to check against on trigger.")]
    [SerializeField] private string playerTag = "Player";

    private Vector2 direction;
    private float speed;
    private bool launched = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 dir, float launchSpeed)
    {
        direction = dir.normalized;
        speed = launchSpeed;
        launched = true;

       // Rotate sprite to face travel direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        if (!launched) return;

        // Manual movement, bypassing physics
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            TriggerGameOver();
            Destroy(gameObject);
        }
    }

    private void TriggerGameOver()
    {
        // Hook this up to your GameManager / game-over UI.
        // GameManager.Instance.GameOver();

        Debug.Log("GAME OVER — Mario hit by thrown barrel.");
    }
}