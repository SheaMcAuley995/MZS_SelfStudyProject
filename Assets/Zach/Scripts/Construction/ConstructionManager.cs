using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ConstructionData
{
    [Header("Components and Dependencies")]
    public Animator animator;
    public Inventory inventory;
    public ThirdPersonCamera cam;
    public Transform player;
    public Transform buildPos;

    [Header("Building Conditions")]
    public Collider[] blockers;
    public LayerMask buildMask;
    [HideInInspector] public bool blueprinted;
    [HideInInspector] public bool constPanelOpen;

    [Header("Input")]
    public KeyCode constructionButton;
    [HideInInspector] public KeyCode defaultConstButton;

    [Header("Buildings")]
    public GameObject[] transparentBuilding;
    public GameObject[] builtBuilding;
    [HideInInspector] public GameObject buildingBlueprint;
    [HideInInspector] public int buildingNum;

    [Header("UI")]
    public GameObject constructionPanel;
    public GameObject[] notEnoughScraps;
    public Button[] buildingButtons;

    [Header("Settings")]
    public float buildDistance;
    public float overlapDistance;
    public int costOfTurrets;
    public int costOfStorage;
}

public class ConstructionManager : MonoBehaviour
{
    public ConstructionData data;
    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

	// Use this for initialization
	void Start ()
    {
        FindComponents();
        SetVariables();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(data.constructionButton) && !data.constPanelOpen && !data.blueprinted && !Rocket.instance.inspectingRocket)
        {
            OpenConstructionPanel();
        }
        else if (Input.GetKeyDown(data.constructionButton) && data.constPanelOpen)
        {
            CloseConstructionPanel();
        }

        // If a blueprint is active
        if (data.blueprinted)
        {
            // Used to prevent building ontop of other objects
            #region Build Condition
            bool canBuildHere;
            data.blockers = Physics.OverlapSphere(data.buildPos.transform.position, data.overlapDistance, data.buildMask, QueryTriggerInteraction.Collide);

            // If there are no objects in the location...
            if (data.blockers.Length == 0)
            {
                // Player can build there!
                canBuildHere = true;
            }
            // Otherwise...
            else
            {
                // They can't
                canBuildHere = false;
            }
            #endregion

            // If blueprint is active and player presses escape...
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Cancels the blueprint
                DestroyBlueprint();
            }

            // If a blueprint is active and the player clicks...
            if (Input.GetButtonDown("Fire1"))
            {
                // If it's a valid build location...
                if (canBuildHere)
                {
                    if (data.buildingNum == 1)
                    {
                        // Builds the turret
                        BuildTurret();
                    }
                    else if (data.buildingNum == 2)
                    {
                        // Builds the storage
                        BuildStorage();
                    }
                }
                else
                {
                    // Error message/noise/effect
                }
            }
        }
	}

    // Opens the build panel
    void OpenConstructionPanel()
    {
        data.constPanelOpen = true;
        data.animator.SetBool("construction", true);
        //data.constructionPanel.SetActive(true);
        data.cam.UnlockCursor();
        data.cam.camData.mouseSensitivity = 0f;

        #region Turret Resource Requirement
        if (data.inventory.numberOfScraps < data.costOfTurrets)
        {
            data.buildingButtons[0].interactable = false;
            data.notEnoughScraps[0].SetActive(true);
        }
        else
        {
            data.buildingButtons[0].interactable = true;
            data.notEnoughScraps[0].SetActive(false);
        }
        #endregion

        #region Storage Resource Requirement
        if (data.inventory.numberOfScraps < data.costOfStorage)
        {
            data.buildingButtons[1].interactable = false;
            data.notEnoughScraps[1].SetActive(true);
        }
        else
        {
            data.buildingButtons[1].interactable = true;
            data.notEnoughScraps[1].SetActive(false);
        }
        #endregion
    }

    // Closes the build panel
    void CloseConstructionPanel()
    {
        data.constPanelOpen = false;
        data.animator.SetBool("construction", false);
        //data.constructionPanel.SetActive(false);
        data.cam.LockCursor();
        data.cam.camData.mouseSensitivity = 10f;
    }

    // Selects turret for construction, spawns the blueprint 
    public void SelectTurret()
    {
        CloseConstructionPanel();
        data.blueprinted = true;
        data.buildingNum = 1;

        Vector3 pos = data.buildPos.transform.position;
        data.buildingBlueprint = Instantiate(data.transparentBuilding[0], pos, Quaternion.identity, data.player.transform);
    }

    // Selects storage for construction, spawns the blueprint
    public void SelectStorage()
    {
        CloseConstructionPanel();
        data.blueprinted = true;
        data.buildingNum = 2;

        Vector3 pos = data.buildPos.transform.position;
        data.buildingBlueprint = Instantiate(data.transparentBuilding[1], pos, Quaternion.identity, data.player.transform);
    }

    // Spawns turret at set location
    void BuildTurret()
    {
        data.inventory.numberOfScraps -= data.costOfTurrets;

        Destroy(data.buildingBlueprint);
        data.blueprinted = false;

        Vector3 pos = data.buildPos.transform.position;
        GameObject turret = Instantiate(data.builtBuilding[0], pos, Quaternion.identity);
    }

    // Spawns storage at set location
    void BuildStorage()
    {
        data.inventory.numberOfScraps -= data.costOfStorage;

        Destroy(data.buildingBlueprint);
        data.blueprinted = false;

        Vector3 pos = data.buildPos.transform.position;
        GameObject storage = Instantiate(data.builtBuilding[1], pos, Quaternion.identity);
    }

    // Destroys the blueprint building if player cancels construction
    void DestroyBlueprint()
    {
        Destroy(data.buildingBlueprint);
        data.blueprinted = false;
    }

    #region Start Functions
    void FindComponents()
    {
        data.inventory = FindObjectOfType<Inventory>();
        data.cam = FindObjectOfType<ThirdPersonCamera>();
    }

    void SetVariables()
    {
        data.defaultConstButton = KeyCode.Tab;
        if (data.constructionButton == KeyCode.None)
        {
            data.constructionButton = data.defaultConstButton;
        }

        data.buildDistance = 5f;
        data.overlapDistance = 1f;
        data.costOfTurrets = 40;
        data.costOfStorage = 100;
    }
    #endregion
}
