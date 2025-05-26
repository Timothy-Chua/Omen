using UnityEngine;

public class CatDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider actor)
    {
        if (CatManager.instance.isStart && !CatManager.instance.isEnd)
        {
            if (actor.gameObject.CompareTag("Cat"))
                CatManager.instance.isCatVisible = true;
        }
    }

    private void OnTriggerStay(Collider actor)
    {
        if (CatManager.instance.isStart && !CatManager.instance.isEnd)
        {
            if (actor.gameObject.CompareTag("Cat"))
                CatManager.instance.isCatVisible = true;
        }
    }

    private void OnTriggerExit(Collider actor)
    {
        if (CatManager.instance.isStart && !CatManager.instance.isEnd)
        {
            if (actor.gameObject.CompareTag("Cat"))
                CatManager.instance.isCatVisible = false;
        }
    }
}
