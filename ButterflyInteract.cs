using UnityEngine;

public class ButterflyInteract : MonoBehaviour
{
    [SerializeField] private bool isBrown = false;
    private float currentTime;
    [SerializeField] private Vector2 targetPos;

    public void Initiate()
    {
        targetPos = new Vector2(transform.position.x, transform.position.y + 12);
        currentTime = 0;
    }

    private void Start()
    {
        Initiate();
    }

    private void OnEnable()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= 2.5f)
            gameObject.SetActive(false);
        else
            currentTime += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, targetPos, ButterflyManager.instance.butterflySpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        if (ButterflyManager.instance.isStart && !ButterflyManager.instance.isEnd)
        {
            ButterflyManager.instance.isWin = isBrown ? true : false;
            ButterflyManager.instance.isEnd = true;
        }
    }
}
