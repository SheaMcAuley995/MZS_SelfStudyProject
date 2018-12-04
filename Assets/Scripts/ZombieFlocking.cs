using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFlocking : MonoBehaviour {

    [SerializeField] Animator anim;

    [SerializeField] float searchRadius;
    [SerializeField] LayerMask searchForLayer;
    
    
    [SerializeField] float updateNewPosition_Timer = 10;
    [SerializeField] float updateNewPosition_Radius = 25;

    float updatePositionTimer = 0;

    bool updateNewPosition = true;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, searchRadius, searchForLayer, QueryTriggerInteraction.Ignore);
        agent.speed = 1.5f;
        foreach (Collider n in hitObjects)
        {
            
            if (n.CompareTag("Player"))
            {
                agent.speed = 7f;
                updateNewPosition = false;
                agent.SetDestination(n.gameObject.transform.position);
            }

           

            //Debug.Log(n);
        }

        Vector3 point;

        if(updateNewPosition)
        {
            if (RandomPoint(transform.position, searchRadius, out point))
            {
                agent.SetDestination(point);
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                updatePositionTimer = agent.remainingDistance + updateNewPosition_Timer;
                if(agent.remainingDistance != 0)
                {
                    //Debug.Log(agent.remainingDistance);
                }
                updateNewPosition = false;
            }
            else
            {
                updateNewPosition = true;
            }
        }
        if (updatePositionTimer > 0)
        {
            updatePositionTimer -= Time.deltaTime;
        }
        else
        {
            updateNewPosition = true;
        }
        float speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("speedPercent", speedPercent, .1f,Time.deltaTime); 
        
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + (Random.insideUnitSphere  * range) + (Vector3.one * range/4);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        
        result = Vector3.zero;
        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, updateNewPosition_Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

}
