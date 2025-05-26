using UnityEngine;

public class CatCamMove : MonoBehaviour
{
    [SerializeField] private Vector2 moveRange;
    [SerializeField] private float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CatManager.instance.isStart && !CatManager.instance.isEnd)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            Vector3 moveDir = transform.right * horizontalInput * moveSpeed * Time.deltaTime;

            transform.position += moveDir;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, moveRange.x, moveRange.y), transform.position.y, transform.position.z);
        }
    }
}
