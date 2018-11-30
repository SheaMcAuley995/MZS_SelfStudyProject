using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public Sprite icon;
    public Transform dumpLocation;

	// Use this for initialization
	void Start ()
    {
        
	}

    public void PickupObject()
    {
        // If it's a scrap...
        if (gameObject.CompareTag("Scrap"))
        {
            Inventory.instance.numberOfScraps += 20;
            Inventory.instance.CheckAmountOfScraps();
        }
        // If it's a (TBD - NYI)
        /*else if (gameObject.CompareTag(""))
        {
            Pickup other object
        }*/
        // ADD MORE AS IMPLEMENTED

        DumpObject();
    }

    // Objects are needed for inventory so they can't be destroyed. Instead, they're moved to where the player won't ever see them, a la Skyrim's unique NPC dump
    void DumpObject()
    {
        transform.position = dumpLocation.position;
    }
}
