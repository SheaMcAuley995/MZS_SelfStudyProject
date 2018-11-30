using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    ConstructionManager construction;
    public LayerMask objectMask;
    public KeyCode[] interactButtons;
    public Transform playerTransform;
    public GameObject itemPickedUp;
    [SerializeField] float distance = 5f;
    KeyCode defaultInteract1 = KeyCode.E;
    KeyCode defaultInteract2 = KeyCode.Return;
    bool detectingObject;
    GameObject objectType;


	// Use this for initialization
	void Start ()
    {
        construction = FindObjectOfType<ConstructionManager>();
        SetDefaultButtons();
	}
	
	// Update is called once per frame
	void Update ()
    {
        detectingObject = CheckForDetection(); // Sets the detection bool to the results of the check for detection method

        if (Input.GetKeyDown(interactButtons[0]) || Input.GetKeyDown(interactButtons[1]))
        {            
            if (detectingObject)
            {
                InteractWithObject();
            }
        }
	}

    // For picking up non-interactable objects
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Pickups>())
        {
            if (!Inventory.instance.inventoryIsFull)
            {
                other.gameObject.GetComponent<Pickups>().PickupObject();
                Inventory.instance.AddItem(itemPickedUp);
            }
            else
            {
                // Inventory full
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Pickups>())
        {
            Inventory.instance.itemAdded = false;
        }
    }

    // Detects objects in range with a raycast, returns true if it detects something that's not a pickup
    bool CheckForDetection()
    {
        RaycastHit hit;
        detectingObject = Physics.Raycast(playerTransform.position, playerTransform.transform.forward, out hit, distance, objectMask, QueryTriggerInteraction.Collide);

        Debug.DrawLine(playerTransform.position, hit.point, Color.red);

        if (hit.collider) // If the detected object has a collider
        {
            objectType = hit.transform.gameObject; // Identifies the GameObject hit for use in the InteractWithObject() method
            return true;
        }
        else
        {
            return false;
        }
    }

    // Player inteacts with the given object, actions depending on which object it is
    void InteractWithObject()
    {
        if (objectType.CompareTag("Rocket"))
        {
            InteractWithRocket();
        }
        else if (objectType.layer == 12)
        {
            InteractWithBuilding();
        }
        else
        {
            // Do code for objects that need interacting
        }
    }

    void InteractWithRocket()
    {
        if (!Rocket.instance.inspectingRocket && !construction.data.blueprinted)
        {
            Rocket.instance.OpenRocketUI();
        }
        else
        {
            Rocket.instance.CloseRocketUI();
        }
    }

    void InteractWithBuilding()
    {
        if (objectType.CompareTag("Turret"))
        {
            // Open deconstruction UI
        }
        else if (objectType.CompareTag("Storage"))
        {
            // Open storage inventory UI
        }
    }

    // Sets the default interact buttons if none are given in the inspector -- DEV TOOL, REMOVE WHEN LAUNCHING!!
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
