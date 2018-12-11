﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool sleeping = false;
    bool eating = true;

    private void Start()
    {
        cur_Food = food_Max;
        cur_Health = health_Max;
        cur_Sleep = sleep_Max;


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
        cur_Food--;
        cur_Sleep--;
        determineState();
        //Debug.Log("Food :" + updateCurrentStatus_Food(cur_Food, food_Max, statusCurves_Food));
        //Debug.Log("Sleep :" + updateCurrentStatus_Food(cur_Sleep, sleep_Max, statusCurves_Sleep));
    }


    private StatusType updateCurrentStatus_Food(float Type_Current, float Type_Max, StatusCurves statusCurves)
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
            throw new System.Exception(statusCurves.ToString() + " status is having some trouble");
        }

    }

    private void determineState()
    {
        float priority_Food = 0;
        float priority_Sleep = 0;

        switch (SleepStatus)
        {
            case StatusType.High:
                priority_Sleep = evaluateStatus(cur_Sleep, sleep_Max) * 0.25f;
                break;
            case StatusType.Med:
                priority_Sleep = evaluateStatus(cur_Sleep, sleep_Max) * 0.5f;
                break;
            case StatusType.Low:
                priority_Sleep = evaluateStatus(cur_Sleep, sleep_Max) * 1;
                break;
        }

        switch (foodStatus)
        {
            case StatusType.High:
                priority_Food = evaluateStatus(cur_Food, food_Max) * 0.25f;
                break;
            case StatusType.Med:
                priority_Food = evaluateStatus(cur_Food, food_Max) * 0.5f;
                break;
            case StatusType.Low:
                priority_Food = evaluateStatus(cur_Food, food_Max) * 1;
                break;
        }
       // Debug.Log("Food Priority :" + priority_Food);
        if (priority_Food > priority_Sleep)
        {
            
            if(sleeping)
            {
                aiBehavior -= Behavior_Sleep;
                sleeping = false;
            }
            if(!eating)
            {
                eating = true;
                aiBehavior += Behavior_Eat;
            }

        }
        else
        {
            //Debug.Log("Sleep Prioirty :" + priority_Sleep);
            if(eating)
            {
                aiBehavior -= Behavior_Eat;
                eating = false;
            }
            if(!sleeping)
            {
                sleeping = true;
                aiBehavior += Behavior_Sleep;
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
        Debug.Log("I am Eating");
        cur_Food+= 2;
    }
    public void Behavior_Sleep()
    {
        Debug.Log("I am Sleeping");
        cur_Sleep += 2;
    }

    #endregion


}
