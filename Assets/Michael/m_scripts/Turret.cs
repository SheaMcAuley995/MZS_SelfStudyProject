using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private Collider[] targets;
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
    public LayerMask mask;
    public AudioSource AttackSound;
    ConstructionManager cm;
    
    int shotsFired = 0;

    float timer = 3f;

    private void Start()
    {
        cm = FindObjectOfType<ConstructionManager>();

    }
    void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackDistance, mask, QueryTriggerInteraction.Ignore);
        targets = enemies;
        timer -= Time.deltaTime;


        if (!cm.data.blueprinted)
        {
            foreach (var target in enemies)
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.red);
                if (Vector3.Distance(transform.position, target.transform.position) < attackDistance)
                {
                    transform.LookAt(target.transform);
                    barrelForward = barrelTip.transform.forward;
                    if (timer <= 0f)
                    {
                        FireTurret();
                        timer = speed;
                    }

                }
            }
        }       
    
    }
    void FireTurret()
    {
        AttackSound.Play();       
        var bullet = Instantiate(projectile, barrelTip.transform.position, barrelTip.transform.rotation) as GameObject;   
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}