using UnityEngine;

public class CemeteryGround : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D actor)
    {
        if (CemeteryManager.instance.isStart && !CemeteryManager.instance.isEnd)
        {
            if (actor.gameObject.CompareTag("Baby"))
            {
                CemeteryManager.instance.isWin = false;
                CemeteryManager.instance.isEnd = true;
            }
        }
    }
}
