using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType { Carnivore, Herbivore, Omnivore }
public enum MoodType { Passive, Neutral, Aggressive }

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

    private void Start()
    {
        cur_Food = food_Max;
        cur_Health = health_Max;
        cur_Sleep = sleep_Max;


        switch(foodType)
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
}
