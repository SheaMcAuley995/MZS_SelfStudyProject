using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needs : MonoBehaviour {

    
    public float sleep = 100;
    public float food = 100;
    public float health = 100;

    private void Update()
    {
        sleep -= Time.deltaTime;
        food -= Time.deltaTime;
        health -= Time.deltaTime;
    }
}
