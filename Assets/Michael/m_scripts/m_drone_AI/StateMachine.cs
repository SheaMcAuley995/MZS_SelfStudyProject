using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States { Wonder, Fetch, Return}
public class StateMachine : MonoBehaviour {

    States state = States.Wonder;
    DroneCollection_AI drone;

    private void Start()
    {
        drone = GetComponent<DroneCollection_AI>();
    }
    private void Update()
    {




        switch (state)
        {
            case States.Wonder:
                Wonder();
                break;
            case States.Fetch:
                Fetch();
                break;
            case States.Return:
                Return();
                break;
        }

    }

    void Wonder()
    {
        drone.Wander();
        //finds scarap then state = States.Fetch;
    }

    void Fetch()
    {

    }

    void Return()
    {

    }
}
