using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroneCollection_AI : MonoBehaviour
{

    [Header(" C a r   &   m o t o r ")]

    public float maxMotorBoost = 1.3f;
    public float motorBoost = 1.1f;
    private float boost = 1.0f;
    public float maxMotorTorque = 100f;
    public float maxSpeed = 37f;
    public float currentSpeed;

    [Header(" b r a k e s ")]
    public Vector3 CenterOfMass;
    public bool isBreaking = false;
    public float brakeDistance = 30.0f;
    public GameObject brakeLights;

    public float maxBrakTorque = 150f;
    public float breakMultiplier;


    

    [Header(" w h e e l s ")]
    public Transform path;
    public float switchDistance = 18f;
    public float maxSwitchDistance = 20f;
    public float minSwitchDistance = 10f;
    public float maxSteerAngle = 45f;
    public float minSteerAngle = 25f;
    private float decidedSteerAngle;
    [HideInInspector]
    public float targetSteerAngle = 0;
    public float turnSpeed = 5.0f;

    [Header(" w h e e l s   c  o l l i d e r ")]
    public WheelCollider fr;
    public WheelCollider fl;
    public WheelCollider br;
    public WheelCollider bl;

    [Header(" s e n s o r s ")]

    public Collider sensorTrigger;
    public Collider grabber;
    public bool foundScrap;
    public int maxTime = 30;
    public int minTime = 5;
    private List<GameObject> nodes;
    private int currentNode = 0;

    [Header("d r o n e  s c r a p s")]
    private int scrapsCollected = 0;
    public int maxScraps;
    public bool scrapsFull;
    public TextMeshPro scrapCounter;
    public List<GameObject> dispNodes;
    public GameObject HomePrism;

    // Use this for initialization
    void Start()
    {
        
        GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        nodes = GetComponentInChildren<ConeDetector>().scraps;
        //Transform[] patrolTransforms = path.GetComponentsInChildren<Transform>();
        //nodes = new List<Transform>();
        //for (int i = 0; i < patrolTransforms.Length; i++)
        //{
        //    if (patrolTransforms[i] != path.transform)
        //    { nodes.Add(patrolTransforms[i]); }
        //}

        boost = UnityEngine.Random.Range(motorBoost, maxMotorBoost);
        switchDistance = UnityEngine.Random.Range(maxSwitchDistance, minSwitchDistance);
        decidedSteerAngle = UnityEngine.Random.Range(maxSteerAngle, minSteerAngle);

    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        Drive();
        Wander();
        Fetch();
        ReturnScraps();
        //ApplySteering();
        //ApplyBrakes();
        CheckWaypoint();
        //SlowOnApproach();
        Sense();
        dispNodes = nodes;
        if (scrapsCollected == maxScraps)
        {
            scrapsFull = true;
        }
    }

    private void ReturnScraps()
    {
        if (scrapsFull)
        {

            
            Vector3 relativeVector = transform.InverseTransformPoint(HomePrism.transform.position);

            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            targetSteerAngle = newSteer;
            fl.steerAngle = Mathf.Lerp(fl.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
            fr.steerAngle = Mathf.Lerp(fr.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
            
        }
    }

    private void Sense()
    {
        nodes = GetComponentInChildren<ConeDetector>().scraps;
        if (nodes.Count > 0)
        {
            foundScrap = true;
            if (nodes[currentNode].gameObject.activeSelf == false)
            {
                nodes.Remove(nodes[currentNode]);
            }
            
        }

    }
    private void Fetch()
    {
        if (foundScrap && !scrapsFull)
        {
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].transform.position);

            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            targetSteerAngle = newSteer;
            fl.steerAngle = Mathf.Lerp(fl.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
            fr.steerAngle = Mathf.Lerp(fr.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
            if (nodes.Count <= 0)
            {
                foundScrap = false;
            }
        }
        
    }
    private void ApplyBrakes()
    {
        breakMultiplier = currentSpeed / brakeDistance;
        if (isBreaking)
        {
            br.brakeTorque = maxBrakTorque * breakMultiplier;
            bl.brakeTorque = maxBrakTorque * breakMultiplier;
            brakeLights.SetActive(true);
        }
        else
        {
            br.brakeTorque = 0;
            bl.brakeTorque = 0;
            brakeLights.SetActive(false);
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(transform.position, nodes[currentNode].transform.position);
    //}
    private void ApplySteering()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].transform.position);

        float newSteer = (relativeVector.x / relativeVector.magnitude) * decidedSteerAngle;
        targetSteerAngle = newSteer;       
        fl.steerAngle = Mathf.Lerp(fl.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        fr.steerAngle = Mathf.Lerp(fr.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }
    IEnumerator WanderDirectionChange()
    {
        int changeTime = UnityEngine.Random.Range(minTime, maxTime);
        targetSteerAngle = UnityEngine.Random.Range(minSteerAngle, maxSteerAngle);
        yield return new WaitForSecondsRealtime(changeTime);
       
    }
    private void Drive()
    {
        currentSpeed = (2 * Mathf.PI * fl.radius * fl.rpm * 60 / 1000);

        if (currentSpeed < maxSpeed && !isBreaking)
        {

            bl.motorTorque = (maxMotorTorque * boost);
            br.motorTorque = (maxMotorTorque * boost);
        }
        else
        {
            // isBreaking = true;
            fl.motorTorque = 0;
            fr.motorTorque = 0;
            bl.motorTorque = 0;
            br.motorTorque = 0;

        }

    }
    private void CheckWaypoint()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].transform.position) < switchDistance)
        {

            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                if (nodes[currentNode].gameObject.activeSelf == false)
                {
                    nodes.Remove(nodes[currentNode]);
                }
                currentNode++;
                scrapsCollected++;
                scrapCounter.text = scrapsCollected.ToString();
            }
            if (nodes.Count == 0)
            {
                foundScrap = false;
            }
        }
    }
    public void Wander()
    {

        if (!foundScrap && !scrapsFull)
        {
            Vector3 relativeVector = transform.InverseTransformPoint(transform.forward);

            float newSteer = (relativeVector.x / relativeVector.magnitude) * decidedSteerAngle;
            StartCoroutine(WanderDirectionChange());
            targetSteerAngle = newSteer;

            fl.steerAngle = Mathf.Lerp(fl.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
            fr.steerAngle = Mathf.Lerp(fr.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        }
        
    }
    private void SlowOnApproach()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].transform.position) < brakeDistance)
        {
            StartCoroutine(doBrakes());

        }
        else { isBreaking = false; }
    }
    IEnumerator doBrakes()
    {
        isBreaking = true;
        yield return new WaitForSecondsRealtime(2);
        isBreaking = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            nodes.Remove(other.gameObject);
            scrapsCollected++;
            scrapCounter.text = scrapsCollected.ToString();
            other.gameObject.SetActive(false);
        }
    }
}
