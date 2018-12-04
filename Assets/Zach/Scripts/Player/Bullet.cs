using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Parameters")]
    public float speed = 100f;
    public float damage = 20f;
    public float impactForce = 50f;

    [Header("Effects")]
    public GameObject impactEffect;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        BulletBehavior();
    }

    void BulletBehavior()
    {
        rb.AddForce(PlayerGun.instance.firePoint.transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

    

   //void OnCollisionEnter(Collision other)
   //{
   //    //GameObject hitEffect = Instantiate(impactEffect, other.transform.position, Quaternion.identity);
   //    Destroy(gameObject);
   //
   //    if (other.gameObject.CompareTag("Enemy"))
   //    {          
   //        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
   //
   //        if (other.gameObject.GetComponent<Rigidbody>() != null)
   //        {
   //           other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(impactForce, transform.position, 5f);              
   //        }
   //    }     
   //}
}
