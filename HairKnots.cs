using UnityEngine;

public class HairKnots : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (HairManager.instance.isStart && !HairManager.instance.isEnd)
        {
            HairManager.instance.currentKnots++;
            gameObject.SetActive(false);
        }
    }
}
