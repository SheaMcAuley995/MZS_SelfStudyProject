using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothedCamera : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed;

    public Vector3 offset;

    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Swap());
        }

        Vector3 worldOff = target.TransformVector(offset);
        Vector3 targetpos = target.position + worldOff;
        Vector3 desiredPos = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * smoothSpeed);
        transform.position = desiredPos;

        transform.LookAt(target);
        //offset.y += Input.GetAxisRaw("Mouse ScrollWheel")* 100;

        offset.z += Input.GetAxisRaw("Mouse ScrollWheel") * 50;
        //offset.x = Mathf.Clamp(offset.x, -150, -25);
    }
    IEnumerator Swap()
    {
        float swapTemp = offset.x;
        swapTemp = offset.x * -1f;
        offset.x = swapTemp;
        yield return new WaitForSecondsRealtime(1);
    }
}