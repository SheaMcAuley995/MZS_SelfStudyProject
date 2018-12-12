using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [Header("Components")]
    public Transform objectToFollow;
    public Vector3 offset;

    [Header("Settings")]
    public float followSpeed = 10f;
    public float lookSpeed = 10f;


    void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }

    void LookAtTarget()
    {
        Vector3 lookDirection = objectToFollow.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
    }

    void MoveToTarget()
    {
        Vector3 targetPos = objectToFollow.position + objectToFollow.forward * offset.z + objectToFollow.right * offset.x + objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
