using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float turningRate;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject barrelTip;
    //private GameObject temp;
    public float attackDistance;
    private Vector3 barrelForward;

    public AudioSource AttackSound;

    int shotsFired = 0;

    float timer = 3f;
    void Update()
    {
       
     
        timer -= Time.deltaTime;

      //  print(timer);
        Debug.DrawLine(transform.position, target.transform.position, Color.red);


            if (Vector3.Distance(transform.position, target.transform.position) < attackDistance)
            {
            transform.LookAt(target.transform);
            barrelForward = barrelTip.transform.forward;
                  if (timer <= 0f)
                 {
                
                    //  StartCoroutine("FireDelay");
                         FireTurret();
                     timer = speed;
                  }
               
            }


        

    }






    void FireTurret()
    {
        AttackSound.Play();
        shotsFired++;
        Debug.Log("Shots Fired: " + shotsFired);
        // Instantiate the bullets as gameObjects
        var bullet = Instantiate(projectile, barrelTip.transform.position, barrelTip.transform.rotation) as GameObject;
        // Add the impulse force to make the bullets move
        bullet.GetComponent<Rigidbody>().AddForce(barrelForward * 1000f, ForceMode.Impulse);
    }

    // So that each bullet has a delay between it being fired
    IEnumerator FireDelay()
    {
        FireTurret();
        yield return new WaitForSeconds(speed);
        
    }
    IEnumerator Fire()
    {
        yield return StartCoroutine("FireDelay");
    }
}