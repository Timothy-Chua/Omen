using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float catSpeed = 3f;
    [SerializeField] private Transform targetPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = catSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (CatManager.instance.isStart && !CatManager.instance.isEnd)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPos.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
