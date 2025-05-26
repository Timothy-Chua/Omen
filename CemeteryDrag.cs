using UnityEngine;

public class CemeteryDrag : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    // Update is called once per frame
    private void OnMouseDown()
    {
        if (CemeteryManager.instance.isStart && !CemeteryManager.instance.isEnd)
        {
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }

    private void OnMouseDrag()
    {
        if (CemeteryManager.instance.isStart && !CemeteryManager.instance.isEnd)
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

            // cursorPosition.x = transform.position.x;
            // cursorPosition.z = transform.position.z;

            transform.position = cursorPosition;
        }
    }
}
