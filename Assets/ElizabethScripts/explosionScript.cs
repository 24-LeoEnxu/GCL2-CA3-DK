using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class explosionScript : MonoBehaviour
{
    public Animator explosionAnimator;
    public GameObject explodingObject;
    public SpriteRenderer objectSR;
    public LevelManagerScript levelManagerScript;
    //public Health playerHealth; //this used only for player health system, not for instant respawn system



    private bool playerNear = false;
    //public bool objectNear = false;
    //private explosionScript nearbyExplosive;
    //private explosionScript nearbyExplosiveScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explosionAnimator = GetComponent<Animator>();
        objectSR = explodingObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Explode()
    {
        explosionAnimator.SetTrigger("explosion");
        print("BOOM!");
        StartCoroutine(DestroyDelay(2.0f));


    }

    public void TriggerExplosion(int timer)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < timer; i++)
        {
            StartCoroutine(Flashing());

        }

        StartCoroutine(ExplosionCountdown(timer));
    }

    public void Kaboom(int damage)
    {
        if (playerNear == true)
        {
            //playerHealth.TakeDamage(damage); //this used only for player health system, not for instant respawn system
            levelManagerScript.marioDie();
        }
        /*if (objectNear == true)
        {
            nearbyExplosive.Explode(); 
        }*/

    }




    public IEnumerator ExplosionCountdown(int duration)
    {
        print("start explosion countdown");
        yield return new WaitForSeconds(duration);
        Explode();
    }


    public IEnumerator DestroyDelay(float duration)
    {
        print("start delay");
        yield return new WaitForSeconds(duration / 2);
        objectSR.enabled = false;
        yield return new WaitForSeconds(duration / 2);
        Destroy(explodingObject);

    }

    public IEnumerator Flashing()
    {
        objectSR.color = new Color(255.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.5f);
        objectSR.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.5f);
    }




    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerNear = true;
        }
        /*if (other.tag == "Object")
        {
            objectNear = true;
            nearbyExplosive = other.GetComponentInChildren<explosionScript>(true); //explosions will set off nearby explosives too
            nearbyExplosive.gameObject.SetActive(true);
            
        }*/
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        playerNear = false;
        //objectNear = false;
    }
}
