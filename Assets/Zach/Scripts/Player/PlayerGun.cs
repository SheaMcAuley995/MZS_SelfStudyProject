using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject firePoint;
    public GameObject impactEffect;
    public GameObject projectile;
    public GameObject projectileParent;

    public float speed = 5f;
    public float range = 15f;
    public float reloadTime = 3f;
    bool isReloading;
    float nextTimeToFire = 0f;

    public float fireRate = 15f;
    public float impactForce = 20f;
    int ammo;
    public int maxAmmo = 10;


    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetButton("Fire1") && ammo != 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectile, firePoint.transform.position, Quaternion.identity, projectileParent.transform);
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * speed, ForceMode.Impulse);
        Destroy(bullet, 3f);
    }
}
