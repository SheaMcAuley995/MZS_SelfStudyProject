using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    #region singleton

    public static Rocket instance;

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

    ThirdPersonController controller;
    ThirdPersonCamera cam;
    public GameObject rocketPanel;
    public GameObject itemToPickUp;
    public Text amountText;
    public Image completionBar;
    public Text completionPercent;
    public float totalScrapsNeeded = 1000f;
    public float scrapsContributed;
    [HideInInspector] public bool inspectingRocket;
    int amountToContribute = 0;

	// Use this for initialization
	void Start ()
    {
        controller = FindObjectOfType<ThirdPersonController>();
        cam = FindObjectOfType<ThirdPersonCamera>();
	}

    public void OpenRocketUI()
    {
        rocketPanel.SetActive(true);
        inspectingRocket = true;

        controller.enabled = false;
        cam.UnlockCursor();
        cam.camData.mouseSensitivity = 0f;
    }

    public void CloseRocketUI()
    {
        rocketPanel.SetActive(false);
        inspectingRocket = false;

        controller.enabled = true;
        cam.LockCursor();
        cam.camData.mouseSensitivity = 10f;

        amountToContribute = 0;
        amountText.text = "+ " + amountToContribute;
        amountText.color = Color.yellow;
    }

    public void AddScraps()
    {
        amountToContribute += 20;

        if (amountToContribute == 0)
        {
            amountText.text = "+ " + amountToContribute;
            amountText.color = Color.yellow;
        }
        else if (amountToContribute < 0)
        {
            amountText.text = "- " + amountToContribute;
            amountText.color = Color.red;
        }
        else if (amountToContribute > 0)
        {
            amountText.text = "+ " + amountToContribute;
            amountText.color = Color.green;
        }        
    }

    public void RemoveScraps()
    {
        amountToContribute -= 20;

        if (amountToContribute == 0)
        {
            amountText.text = "+ " + amountToContribute;
            amountText.color = Color.yellow;
        }
        else if (amountToContribute < 0)
        {
            amountText.text = "- " + amountToContribute;
            amountText.color = Color.red;
        }
        else if (amountToContribute > 0)
        {
            amountText.text = "+ " + amountToContribute;
            amountText.color = Color.green;
        }       
    }

    // Contribute scraps to the rocket
    public void Contribute()
    {
        // If the player is trying to contribute more scraps than they have...
        if (amountToContribute <= Inventory.instance.numberOfScraps)
        {
            // If the player is trying to remove scraps and there are no scraps to remove...
            if (amountToContribute < 0 && scrapsContributed <= 0)
            {
                return; // Exit out of the function
            }

            if (amountToContribute < 0)
            {
                Inventory.instance.AddItem(itemToPickUp);
            }
            else if (amountToContribute > 0)
            {
                Inventory.instance.RemoveItem(itemToPickUp);
            }

            AddOrRemoveScraps();
            HandleUI();

            #region Win
            if (scrapsContributed >= totalScrapsNeeded)
            {
                // WIN
            }
            #endregion

            Inventory.instance.CheckAmountOfScraps();
        }
        else
        {
            // Throw error NYI
        }
    }

    void AddOrRemoveScraps()
    {
        Inventory.instance.numberOfScraps -= amountToContribute;
        scrapsContributed += amountToContribute;
        amountToContribute = 0;
    }

    void HandleUI()
    {
        amountText.text = "+ " + amountToContribute;
        amountText.color = Color.yellow;

        completionBar.fillAmount = scrapsContributed / totalScrapsNeeded;
        completionPercent.text = ((scrapsContributed / totalScrapsNeeded) * 100).ToString() + "%";
    }
}
