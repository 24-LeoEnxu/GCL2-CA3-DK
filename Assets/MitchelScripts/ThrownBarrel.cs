using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))] // needs the trigger collider to detect mario
public class ThrownBarrel : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; // destroys itself after this long just in case it never hits anything
    [SerializeField] private string playerTag = "Player"; // tag to check for on hit

    private Vector2 direction; // which way its flying
    private float speed; // how fast
    private bool launched = false; // makes sure it doesnt move before Launch() is called

    private bool destroyed = false; // check if destroyed

    private void Start()
    {
        Destroy(gameObject, lifetime); // cleanup timer
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
            LevelManagerScript.Instance.playerDeath(); // redirects code to player death in level manager
            Destroy(gameObject); // remove the barrel
        }

        if (other.CompareTag("Hammer"))
        {
            if (destroyed)
                return;

            destroyed = true;

            LevelManagerScript.Instance.AddScore(ScoreType.HammerBarrel, transform.position);
        }
    }
}