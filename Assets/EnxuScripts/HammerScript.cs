using UnityEngine;
using System.Collections;

public class HammerScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Barrel"))
        {
            // trigger sfx
            LevelManagerScript.Instance.play_destroyBarrelSFX();


            // recreate frame freeze upon destroying a barrel

            Destroy(collider.gameObject, 0.3f);
            StartCoroutine(Freeze());

            LevelManagerScript.Instance.AddScore(ScoreType.HammerBarrel);
        }

        if(collider.CompareTag("ThrownBarrel"))
        {
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
