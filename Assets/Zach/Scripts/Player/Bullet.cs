using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Parameters")]
    public float speed = 100f;
    public float damage = 20f;
    public float impactForce = 50f;
    bool isOnTurret;

    [Header("Effects")]
    public GameObject impactEffect;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (!isOnTurret)
        {
            GunBulletBehavior();
        }
        else
        {
            TurretBulletBehavior();
        }
    }

    void GunBulletBehavior()
    {
        rb.AddForce(PlayerGun.instance.firePoint.transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, 3f);
    }

    void TurretBulletBehavior()
    {
        Destroy(gameObject, 3f);
    }
}
