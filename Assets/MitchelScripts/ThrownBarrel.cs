using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))] // needs the trigger collider to detect mario
public class ThrownBarrel : MonoBehaviour
{
    [SerializeField] private float lifetime = 6f; // destroys itself after this long just in case it never hits anything
    [SerializeField] private string playerTag = "Player"; // tag to check for on hit

    private Vector2 direction; // which way its flying
    private float speed; // how fast
    private bool launched = false; // makes sure it doesnt move before Launch() is called

    private void Start()
    {
        Destroy(gameObject, lifetime); // cleanup timer, stops it flying forever if it misses
    }

    public void Launch(Vector2 dir, float launchSpeed)
    {
        direction = dir.normalized; // normalize so speed is consistent regardless of distance
        speed = launchSpeed;
        launched = true; // now Update() will actually move it

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // math to rotate sprite towards travel dir
        transform.rotation = Quaternion.Euler(0f, 0f, angle); // apply the rotation
    }

    private void Update()
    {
        if (!launched) return; // dont move until launched

        transform.position += (Vector3)(direction * speed * Time.deltaTime); // move manually, no rigidbody so ignores platforms n stuff
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag)) // hit mario
        {
            TriggerGameOver(); // end the game
            Destroy(gameObject); // remove the barrel
        }
    }

    private void TriggerGameOver()
    {
        // TODO: hook this into an actual GameManager later
        Debug.Log("GAME OVER — Mario hit by thrown barrel."); // just a placeholder for now
        Time.timeScale = 0f; // freezes everything so i can see it worked
    }
}