using UnityEngine;
using System.Collections;

public class HammerScript : MonoBehaviour
{
    private bool hasBeenHit = false; // such that when barrel hits mario, it will help ignore that it has 2 colliders

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if (hasBeenHit)
           // return;

        if (collider.CompareTag("Barrel"))
        {
            // check boolean to ensure score is calculated once
            //hasBeenHit = true;

            // trigger sfx
            LevelManagerScript.Instance.play_destroyBarrelSFX();


            // recreate frame freeze upon destroying a barrel
            StartCoroutine(Freeze());
            Destroy(collider.gameObject, 0.3f);

            // add score
            LevelManagerScript.Instance.AddScore(ScoreType.HammerBarrel);
        }

        //ADDED A DUPLICATE SO THE THROWN BARRELS ARE INSTANT BREAK SINCE THEY NO NEED BREAK ANIMATIONS
        if (collider.CompareTag("ThrownBarrel"))
        {
            // check boolean to ensure score is calculated once
            //hasBeenHit = true;

            // trigger sfx
            LevelManagerScript.Instance.play_destroyBarrelSFX();


            // recreate frame freeze upon destroying a barrel
            StartCoroutine(Freeze());
            Destroy(collider.gameObject);

            // add score
            LevelManagerScript.Instance.AddScore(ScoreType.HammerBarrel);
        }

        IEnumerator Freeze()
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(0.5f);
            Time.timeScale = 1f;
        }
    }
    
}
