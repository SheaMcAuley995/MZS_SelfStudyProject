using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionManager : MonoBehaviour
{
    Inventory inventory;
    ThirdPersonCamera cam;

    public KeyCode constructionButton;
    public GameObject constructionPanel;
    public GameObject notEnoughScraps;
    public GameObject transparentTurret, builtTurret;
    public Button turretButton; // ADD SHIP PART BUTTONS FOR BUILDING YOUR SHIP
    public Transform player, buildPos;
    public LayerMask buildMask;
    [SerializeField] float buildDistance = 5f;
    [SerializeField] float overlapDistance = 1f;
    GameObject turretBlueprint;
    Collider[] blockers;
    KeyCode defaultConstButton = KeyCode.Tab;
    bool constPanelOpen, blueprinted;

	// Use this for initialization
	void Start ()
    {
        FindComponents();
        SetButton();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(constructionButton) && !constPanelOpen)
        {
            OpenConstructionPanel();
        }
        else if (Input.GetKeyDown(constructionButton) && constPanelOpen)
        {
            CloseConstructionPanel();
        }

        if (blueprinted)
        {
            #region Build Condition
            bool canBuildHere;
            blockers = Physics.OverlapSphere(buildPos.transform.position, overlapDistance, buildMask, QueryTriggerInteraction.Collide);

            if (blockers.Length == 0)
            {
                canBuildHere = true;
            }
            else
            {
                canBuildHere = false;
            }
            #endregion

            // Cancelling the build
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DestroyBlueprint();
            }

            // Build the object
            if (Input.GetButtonDown("Fire1"))
            {
                if (canBuildHere)
                {
                    BuildTurret();
                }
                else
                {
                    // Do something
                }
            }
        }
	}

    void OpenConstructionPanel()
    {
        constPanelOpen = true;
        constructionPanel.SetActive(true);
        cam.UnlockCursor();
        cam.camData.mouseSensitivity = 0f;

        if (inventory.numberOfScraps == 0)
        {
            turretButton.interactable = false;
            notEnoughScraps.SetActive(true);
        }
        else
        {
            turretButton.interactable = true;
            notEnoughScraps.SetActive(false);
        }
    }

    void CloseConstructionPanel()
    {
        constPanelOpen = false;
        constructionPanel.SetActive(false);
        cam.LockCursor();
        cam.camData.mouseSensitivity = 10f;
    }

    public void SelectTurret()
    {
        CloseConstructionPanel();
        blueprinted = true;

        Vector3 pos = buildPos.transform.position;
        turretBlueprint = Instantiate(transparentTurret, pos, Quaternion.identity, player.transform);
    }

    void BuildTurret()
    {
        inventory.numberOfScraps--;

        Destroy(turretBlueprint);
        blueprinted = false;

        Vector3 pos = buildPos.transform.position;
        GameObject turret = Instantiate(builtTurret, pos, Quaternion.identity);
    }

    void DestroyBlueprint()
    {
        Destroy(turretBlueprint);
        blueprinted = false;
    }

    #region Start Functions
    void FindComponents()
    {
        inventory = FindObjectOfType<Inventory>();
        cam = FindObjectOfType<ThirdPersonCamera>();
    }

    void SetButton()
    {
        if (constructionButton == KeyCode.None)
        {
            constructionButton = defaultConstButton;
        }
    }
    #endregion
}
