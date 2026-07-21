using UnityEngine;
using System.Collections;

public class ExplodingBarrelScriptTest : MonoBehaviour
{
    float explodeDefaultTimer;
    float explodeTimer;
    SpriteRenderer sr;

    Animator animator;
    public CircleCollider2D cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explodeDefaultTimer = UnityEngine.Random.Range(5.0f, 10.0f);
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        explodeTimer = explodeDefaultTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Flashing();

        explodeTimer -= Time.deltaTime;

        if (explodeTimer <= 0)
        {
            cc.enabled = true;
            animator.SetBool("BarrelExplode", true);
            explodeTimer = explodeDefaultTimer;
            Destroy(gameObject, 0.7f);
        }
    }

    public IEnumerator Flashing()
    {
        sr.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(0.5f);
        sr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.5f);
    }
}
