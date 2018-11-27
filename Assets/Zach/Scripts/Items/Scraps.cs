using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scraps : MonoBehaviour
{
    Inventory inventory;

    public Collider[] colliders;
    public LayerMask playerMask;
    public KeyCode[] interactButtons;
    [SerializeField] float radius = 5f;
    KeyCode defaultInteract1 = KeyCode.E;
    KeyCode defaultInteract2 = KeyCode.Return;
    bool detectingPlayer;


	// Use this for initialization
	void Start ()
    {
        inventory = FindObjectOfType<Inventory>();

        SetDefaultButtons();
    }
	
	// Update is called once per frame
	void Update ()
    {
        detectingPlayer = PlayerInRange();

        if (detectingPlayer)
        {
            if (Input.GetKeyDown(defaultInteract1) || Input.GetKeyDown(defaultInteract2))
            {
                inventory.numberOfScraps++;
                Destroy(gameObject);
                return;
            }
        }
    }

    bool PlayerInRange()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, playerMask);

        if (colliders.Length != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
