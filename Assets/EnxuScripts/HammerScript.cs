using UnityEngine;
using System.Collections;

public class HammerScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Barrel") ||  collider.CompareTag("ThrownBarrel"))
        {
            // trigger sfx
            LevelManagerScript.Instance.play_destroyBarrelSFX();


            // recreate frame freeze upon destroying a barrel
            StartCoroutine(Freeze());
            Destroy(collider.gameObject);

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
