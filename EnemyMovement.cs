using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Vector3 targetPosition;
    private int mazeWidth;
    private int mazeHeight;
    private NavMeshAgent navMeshAgent;

    public void Initialize(int width, int height)
    {
        mazeWidth = width;
        mazeHeight = height;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        SetRandomTargetPosition();
    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    void SetRandomTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(0, mazeWidth), 0, Random.Range(0, mazeHeight));
        navMeshAgent.SetDestination(targetPosition);
    }
}
