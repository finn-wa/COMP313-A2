using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Panda;

public class EnemyTasks : MonoBehaviour
{
    public bool paused = false;
    public Transform player;
    public float range = 6.5f;
    public float patrolSpeed = 5;
    public float chaseSpeed = 8;
    public float fieldOfVision = 60;
    public Transform[] patrolPoints;
    public UnityEvent attack;
    public UnityEvent redLight;
    public UnityEvent blueLight;
    NavMeshAgent agent;
    Transform agentTransform;
    float timerEndsAt = 0f;
    int currentIndex = 0;
    float requiredProximity = 0.25f;

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
    void MoveToNextPoint()
    {
        if (currentIndex >= patrolPoints.Length)
        {
            currentIndex = 0;
        }
        agent.SetDestination(patrolPoints[currentIndex].position);
        currentIndex++;
        Task.current.Succeed();

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
    void RedLight()
    {
        redLight.Invoke();
        Task.current.Succeed();
    }

    [Task]
    void BlueLight()
    {
        blueLight.Invoke();
        Task.current.Succeed();
    }

    [Task]
    void Die()
    {
        agent.isStopped = true;
        Task.current.Succeed();
    }
}
