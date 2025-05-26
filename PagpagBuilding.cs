using UnityEngine;

public class PagpagBuilding : MonoBehaviour
{
    [SerializeField] private bool isConvenience = false;

    private void OnTriggerStay2D(Collider2D actor)
    {
        if (actor.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && PagpagManager.instance.isStart && !PagpagManager.instance.isEnd)
            {
                PagpagManager.instance.isWin = isConvenience ? true : false;
                PagpagManager.instance.isEnd = true;
                PagpagManager.instance.player.gameObject.SetActive(false);
            }
        }
    }
}
