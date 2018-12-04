using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_NeedsMotor : MonoBehaviour {

    [SerializeField]
    public Needs needs;

    private void Start()
    {
        needs = new Needs();
    }


    private void Update()
    {
        needs.sleep -= Time.deltaTime;

        Debug.Log(needs.sleep.ToString());
    }
}
