using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FoodType { Carnivore, Herbivore, Omnivore }
public enum MoodType { Passive, Neutral, Aggressive }
public enum StatusType { High, Med, Low }

[System.Serializable]
struct StatusCurves
{
    public AnimationCurve status_High;
    public AnimationCurve status_Med;
    public AnimationCurve status_Low;
}


public class Ai_Behaviors : MonoBehaviour {

    delegate void AiBehavior();
    AiBehavior aiBehavior;

    [Header("Statistics")]

    [SerializeField] float sleep_Max = 100;
    [SerializeField] float food_Max = 100;
    [SerializeField] float health_Max = 100;
    [SerializeField] float speed_Max = 5;

    [Space]
    [Header("Behavior Types")]
    [SerializeField] FoodType foodType;
    [SerializeField] MoodType moodType;

    private float cur_Sleep;
    private float cur_Food;
    private float cur_Health;

    [Space]
    [Header("Status Curves")]
    [SerializeField] StatusCurves statusCurves_Sleep;
    [SerializeField] StatusCurves statusCurves_Food;
    [SerializeField] StatusCurves statusCurves_Health;

    private StatusType foodStatus = StatusType.High;
    private StatusType SleepStatus = StatusType.High;
    private StatusType HealthStatus = StatusType.High;

    [SerializeField] float statChangeSpeed = 0.1f;

    [Space][Header("Tarets")]
    GameObject curTarget;
    [SerializeField] GameObject sleepTarget;
    [SerializeField] GameObject eatTarget;

    bool sleeping = false;
    bool eating = false;
    bool wondering = false;
    float priority_Food = 0;
    float priority_Sleep = 0;
 
    NavMeshAgent agent;

    [Space][Header("Wander stats")]
    bool updateNewPosition = true;
    [SerializeField] float searchRadius;
    [SerializeField] LayerMask searchForLayer;
    [SerializeField] float updateNewPosition_Timer = 10;
    [SerializeField] float updateNewPosition_Radius = 25;
    float updatePositionTimer = 0;

    private void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void Start()
    {
        cur_Food = food_Max - 1;
        cur_Health = health_Max;
        cur_Sleep = sleep_Max;

       //agent = GetComponent<NavMeshAgent>();
        
        switch (foodType)
        {
            case FoodType.Carnivore:
                aiBehavior += Behavior_Carnivore;
                break;
            case FoodType.Herbivore:
                aiBehavior += Behavior_Herbivore;
                break;
            case FoodType.Omnivore:
                aiBehavior += Behavior_Omnivore;
                break;
        }
        switch (moodType)
        {
            case MoodType.Aggressive:
                aiBehavior += Behavior_Aggressive;
                break;
            case MoodType.Neutral:
                aiBehavior += Behavior_Neutral;
                break;
            case MoodType.Passive:
                aiBehavior += Behavior_Passive;
                break;
        }
        aiBehavior();
    }


    private void Update()
    {
        foodStatus = updateCurrentStatus(cur_Food, food_Max, statusCurves_Food);
        SleepStatus = updateCurrentStatus(cur_Sleep, sleep_Max, statusCurves_Sleep);

        determineState();
        aiBehavior();
        

        if (Vector3.Distance(sleepTarget.transform.position, transform.position) > 2)
        {
            cur_Sleep -= statChangeSpeed * Time.deltaTime;
        }
        else
        {
            cur_Sleep += statChangeSpeed * 5 * Time.deltaTime;
        }

        if (Vector3.Distance(eatTarget.transform.position, transform.position) > 2)
        {
            cur_Food -= statChangeSpeed * Time.deltaTime;
        }
        else
        {
            cur_Food += statChangeSpeed * 5 * Time.deltaTime;
        }

        cur_Food = Mathf.Clamp(cur_Food, 0, food_Max);
        cur_Sleep = Mathf.Clamp(cur_Sleep, 0, sleep_Max);
        //Debug.Log("Food :" + updateCurrentStatus_Food(cur_Food, food_Max, statusCurves_Food));
        //Debug.Log("Sleep :" + updateCurrentStatus_Food(cur_Sleep, sleep_Max, statusCurves_Sleep));
        

    }


    private StatusType updateCurrentStatus(float Type_Current, float Type_Max, StatusCurves statusCurves)
    {
        if (statusCurves.status_High.Evaluate(evaluateStatus(Type_Current, Type_Max)) > statusCurves.status_Med.Evaluate(evaluateStatus(Type_Current, Type_Max)) &&
            statusCurves.status_High.Evaluate(evaluateStatus(Type_Current, Type_Max)) > statusCurves.status_Low.Evaluate(evaluateStatus(Type_Current, Type_Max)))
            return StatusType.High;

        else if (statusCurves.status_Med.Evaluate(evaluateStatus(Type_Current, Type_Max)) > statusCurves.status_High.Evaluate(evaluateStatus(Type_Current, Type_Max)) &&
            statusCurves.status_Med.Evaluate(evaluateStatus(Type_Current, Type_Max)) > statusCurves.status_Low.Evaluate(evaluateStatus(Type_Current, Type_Max)))
            return StatusType.Med;

        else if (statusCurves.status_Low.Evaluate(evaluateStatus(Type_Current, Type_Max)) > statusCurves.status_High.Evaluate(evaluateStatus(Type_Current, Type_Max)) &&
             statusCurves.status_Low.Evaluate(evaluateStatus(Type_Current, Type_Max)) > statusCurves.status_Med.Evaluate(evaluateStatus(Type_Current, Type_Max)))
            return StatusType.Low;

        else
        {
            return StatusType.Low;
        }

    }

    private void determineState()
    {

        switch (SleepStatus)
        {
            case StatusType.High:
                priority_Sleep = 1 - evaluateStatus(cur_Sleep, sleep_Max);
                break;
            case StatusType.Med:
                priority_Sleep = 1 - evaluateStatus(cur_Sleep, sleep_Max);
                break;
            case StatusType.Low:
                priority_Sleep = 1 - evaluateStatus(cur_Sleep, sleep_Max);
                break;
        }

        switch (foodStatus)
        {
            case StatusType.High:
                priority_Food = 1 - evaluateStatus(cur_Food, food_Max);
                break;
            case StatusType.Med:
                priority_Food = 1 - evaluateStatus(cur_Food, food_Max);
                break;
            case StatusType.Low:
                priority_Food = 1 - evaluateStatus(cur_Food, food_Max);
                break;
        }

        //priority_Sleep = Mathf.Clamp01(priority_Sleep);
        //priority_Food = Mathf.Clamp01(priority_Food);

        if (foodStatus == StatusType.High && SleepStatus == StatusType.High)
        {
            if (!wondering)
            {
                aiBehavior += Behavior_Wander;
                wondering = true;
            }
            if (sleeping)
            {
                aiBehavior -= Behavior_Sleep;
                sleeping = false;
            }
            if (eating)
            {
                aiBehavior -= Behavior_Eat;
                eating = false;
            }
        }
        else if (priority_Sleep >= priority_Food)
        {
            if (wondering)
            {
                aiBehavior -= Behavior_Wander;
                wondering = false;
            }
            if (!sleeping)
            {
                aiBehavior += Behavior_Sleep;
                sleeping = true;
            }
            if (eating)
            {
                aiBehavior -= Behavior_Eat;
                eating = false;
            }
        }
        else if (priority_Sleep < priority_Food)
        {
            if (wondering)
            {
                aiBehavior -= Behavior_Wander;
                wondering = false;
            }
            if (sleeping)
            {
                aiBehavior -= Behavior_Sleep;
                sleeping = false;
            }
            if (!eating)
            {
                aiBehavior += Behavior_Eat;
                eating = true;
            }
        }
    }

    private float evaluateStatus(float currentStatus, float statusMax)
    {
        return currentStatus / statusMax;
    }

    

    #region Behaviors
    public void Behavior_Carnivore()
    {

        Debug.Log("I eat meat");
    }
    public void Behavior_Herbivore()
    {
        Debug.Log("I eat plants");
    }
    public void Behavior_Omnivore()
    {
        Debug.Log("I eat anything");
    }
    public void Behavior_Passive()
    {
        Debug.Log("I won't attack back");
    }
    public void Behavior_Neutral()
    {
        Debug.Log("I will defend myself");
    }
    public void Behavior_Aggressive()
    {
        Debug.Log("I will attack on sight");
    }

    public void Behavior_Eat()
    {
        curTarget = eatTarget;
        agent.SetDestination(curTarget.transform.position);
    }
    public void Behavior_Sleep()
    {
        curTarget = sleepTarget;
        agent.SetDestination(curTarget.transform.position);
    }
    public void Behavior_Wander()
    {
        Vector3 point;

        if (updateNewPosition)
        {
            if (RandomPoint(transform.position, searchRadius, out point))
            {
                agent.SetDestination(point);
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                updatePositionTimer = agent.remainingDistance + updateNewPosition_Timer;
                if (agent.remainingDistance != 0)
                {

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
               
        //anim.SetFloat("speedPercent", speedPercent, .1f, Time.deltaTime);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + (Random.insideUnitSphere * range);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    #endregion


}
