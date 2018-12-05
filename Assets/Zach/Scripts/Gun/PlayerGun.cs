using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    #region singleton

    public static PlayerGun instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [Header("Firing GameObjects")]
    public GameObject firePoint;   
    public GameObject projectile;
    public GameObject projectileParent;

    [Header("Components")]
    public AudioSource gunShot;   
    ConstructionManager construction;

    [Header("Aiming Components")]
    public Vector3 gun;
    public Transform hipPosition;
    public Transform aimingPosition;
    public bool aimingDownSights = false;
    Transform cam;

    [Header("Reloading Parameters")]
    public float reloadTime = 3f;
    bool isReloading;

    [Header("Ammunition")]
    public int maxAmmo = 10;
    int ammo;


    void Start()
    {
        RefillAmmo();
        construction = FindObjectOfType<ConstructionManager>();
        cam = Camera.main.transform;
        gun = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        Shoot();
        //Aim();
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading && !construction.data.blueprinted && !construction.data.constPanelOpen && !Rocket.instance.inspectingRocket)
        {
            GameObject bullet = Instantiate(projectile, firePoint.transform.position, Quaternion.LookRotation(firePoint.transform.forward, firePoint.transform.up), projectileParent.transform);
            ammo--;

            if (ammo <= 0)
            {
                StartCoroutine(Reload());
            }

            gunShot.Play();
        }
    }

    void Aim()
    {
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        RefillAmmo();      
        yield break;
    }

    void RefillAmmo()
    {
        ammo = maxAmmo;
        isReloading = false;
    }
}
