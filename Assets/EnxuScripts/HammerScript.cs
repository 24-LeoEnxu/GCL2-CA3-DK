using UnityEngine;
using System.Collections;

public class HammerScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Barrel"))
        {
            // trigger sfx
            LevelManagerScript.Instance.play_destroyBarrelSFX();


            // recreate frame freeze upon destroying a barrel
            StartCoroutine(Freeze());
            Destroy(collider.gameObject, 0.3f);
        }

        //ADDED A DUPLICATE SO THE THROWN BARRELS ARE INSTANT BREAK SINCE THEY NO NEED BREAK ANIMATIONS
        if (collider.CompareTag("ThrownBarrel"))
        {
            // trigger sfx
            LevelManagerScript.Instance.play_destroyBarrelSFX();


            // recreate frame freeze upon destroying a barrel
            StartCoroutine(Freeze());
            Destroy(collider.gameObject);
        }

        IEnumerator Freeze()
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(0.5f);
            Time.timeScale = 1f;
        }
    }
    
}
