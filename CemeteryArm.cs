using UnityEngine;

public class CemeteryArm : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D actor)
    {
        if (CemeteryManager.instance.isStart && !CemeteryManager.instance.isEnd)
        {
            if (actor.gameObject.CompareTag("Baby"))
            {
                CemeteryManager.instance.isWin = true;
                CemeteryManager.instance.isEnd = true;
            }
        }
    }
}
