using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f;



    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hitting");
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
