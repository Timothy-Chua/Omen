using UnityEngine;

public class WalisItems : MonoBehaviour
{
    private Outline outline;
    private Color originalColor;
    [SerializeField] private Color colorOnHover = Color.green;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        originalColor = outline.OutlineColor;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline.OutlineColor = originalColor;
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WalisManager.instance.isStart)
        {
            outline.OutlineColor = originalColor;
            outline.enabled = false;
        }

        if (!outline.enabled && WalisManager.instance.isStart && !WalisManager.instance.isEnd)
            outline.enabled = true;

        if (outline.enabled && WalisManager.instance.isStart && WalisManager.instance.isEnd)
            outline.enabled = false;

    }

    private void OnMouseEnter()
    {
        if (WalisManager.instance.isStart && !WalisManager.instance.isEnd)
            outline.OutlineColor = colorOnHover;
    }

    private void OnMouseExit()
    {
        if (WalisManager.instance.isStart && !WalisManager.instance.isEnd)
            outline.OutlineColor = originalColor;
    }

    private void OnMouseDown()
    {
        if (WalisManager.instance.isStart && !WalisManager.instance.isEnd)
        {
            WalisManager.instance.itemsCollected++;
            gameObject.SetActive(false);
        }
    }
}
