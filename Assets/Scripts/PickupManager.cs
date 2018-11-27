using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public GameObject[] rocketParts;

	// Use this for initialization
	IEnumerator Start ()
    {
        yield return new WaitForSeconds(1f);
        FindPickups();
	}

    void FindPickups()
    {
        rocketParts = GameObject.FindGameObjectsWithTag("Pickup");
    }
}
