using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public LayerMask Item;
    public KeyCode[] interactButtons;
    public Transform playerTransform;
    [SerializeField] float distance = 5f;
    KeyCode defaultInteract1 = KeyCode.E;
    KeyCode defaultInteract2 = KeyCode.Return;
    bool detectingObject;


	// Use this for initialization
	void Start ()
    {
        SetDefaultButtons();
	}
	
	// Update is called once per frame
	void Update ()
    {
        detectingObject = ObjectInRange();

        if (Input.GetKeyDown(interactButtons[0]) || Input.GetKeyDown(interactButtons[1]))
        {            
            if (detectingObject)
            {
                InteractWithObject();
            }
        }
	}

    bool ObjectInRange()
    {
        RaycastHit hit;
        detectingObject = Physics.Raycast(playerTransform.position, playerTransform.transform.forward, out hit, distance, Item, QueryTriggerInteraction.Collide);

        Debug.DrawLine(transform.position, hit.point, Color.red);

        if (hit.collider)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void InteractWithObject()
    {
        Debug.Log("Do Something");
        // Do code for objects that need interacting
    }

    void PickupScraps()
    {
        Inventory.instance.numberOfScraps++;       
    }

    void SetDefaultButtons()
    {
        if (interactButtons[0] == KeyCode.None)
        {
            interactButtons[0] = defaultInteract1;
        }

        if (interactButtons[1] == KeyCode.None)
        {
            interactButtons[1] = defaultInteract2;
        }
    }
}
