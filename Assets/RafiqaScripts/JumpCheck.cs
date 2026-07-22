using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    private barrel_script barrel_script;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Barrel"))
        {
            barrel_script barrel = other.GetComponent<barrel_script>();

            if (barrel != null)
            {
                barrel.AwardJumpScore();
            }
        }
    }
}