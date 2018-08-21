using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class EnemyTasks : MonoBehaviour
{
    public bool paused = true;
    public Transform player;
    public float patrolSpeed = 10;
    public Transform[] patrolPoints;
    NavMeshAgent agent;
    float requiredProximity = 0.25f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.speed = patrolSpeed;
    }

    void Update()
    {
        //animator.SetFloat("Forward", agent.desiredVelocity.magnitude, 0.1f, Time.deltaTime);
    }

    [Task]
    bool Paused()
    {
        return paused;
    }

    /*
     * Move to the destination
     */
    [Task]
    void MoveToPatrolPoint(int pointIndex)
    {
        if (agent != null)
        {
            agent.SetDestination(patrolPoints[pointIndex].position);
            WaitArrival();
        }
    }

    [Task]
    public void WaitArrival()
    {
        var task = Task.current;
        float d = agent.remainingDistance;
        if (agent.velocity.magnitude < 2f && d < 4.5f && !task.isStarting)
        {
            task.Fail();
        }
        if (!task.isStarting && d < requiredProximity)
        {
            task.Succeed();
            d = 0.0f;
        }

        if (Task.isInspected)
            task.debugInfo = string.Format("d-{0:0.00}", d);
    }

    [Task]
    void Die()
    {
        agent.isStopped = true;
        Task.current.Succeed();
    }
}
