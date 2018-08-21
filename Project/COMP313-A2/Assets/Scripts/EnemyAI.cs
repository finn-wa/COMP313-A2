using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Panda;

public class EnemyAI : MonoBehaviour
{
    public bool paused = true;
    public Transform player;
    public float range = 6.5f;
    public float patrolSpeed = 5;
    public float chaseSpeed = 8;
    public float fieldOfVision = 60;
    public Transform[] patrolPoints;
    public UnityEvent attack;
    NavMeshAgent agent;
    Transform agentTransform;
    float requiredProximity = 0.25f;
    float timerEndsAt = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.speed = patrolSpeed;
        agentTransform = GetComponent<Transform>();
    }

    [Task]
    bool Paused()
    {
        return paused;
    }

    [Task]
    void SetTimer(float t)
    {
        timerEndsAt = Time.time + t;
        Task.current.Succeed();
    }

    // Returns true if there is at least t time remaining
    [Task]
    bool TimeRemaining(float t)
    {
        return (timerEndsAt - Time.time) >= t;
    }

        [Task]
    bool TimerFinished()
    {
        return timerEndsAt <= Time.time;
    }

    //Sets destination and then immediately succeeds
    [Task]
    void TargetPatrolPoint(int pointIndex)
    {
        if (agent != null)
        {
            agent.SetDestination(patrolPoints[pointIndex].position);
            Task.current.Succeed();
        }
    }

    // Succeeds on arrival
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
    bool HasArrived()
    {
        return agent.remainingDistance < requiredProximity;
    }

    [Task]
    void Follow()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(player.position);
        Task.current.Succeed();
    }

    [Task]
    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        Task.current.Succeed();
    }

    [Task]
    bool InRange()
    {
        return range >= Vector3.Distance(agentTransform.position, player.position);
    }

    [Task]
    bool InSight()
    {
        Vector3 targetDir = player.position - agentTransform.position;
        float angle = Vector3.Angle(targetDir, agentTransform.forward);
        bool blocked = false;
        RaycastHit[] hits = Physics.RaycastAll(agentTransform.position, targetDir, Vector3.Distance(agentTransform.position, player.position));
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].collider.gameObject.name.StartsWith("Level") &&
                hits[i].transform != player.transform && hits[i].transform != agentTransform)
                {
                    blocked = true;
                }
        }
        bool result = (!blocked && angle < fieldOfVision / 2);
        return result;
    }

    [Task]
    void Attack()
    {
        attack.Invoke();
        Task.current.Succeed();
    }

    [Task]
    void Die()
    {
        agent.isStopped = true;
        Task.current.Succeed();
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
}
