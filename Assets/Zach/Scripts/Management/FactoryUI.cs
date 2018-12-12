using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    #region singleton
    public static FactoryUI instance;

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

    [Header("Dependencies")]
    public GameObject dronePrefab;
    public GameObject roverPrefab;
    public Transform robotSpawnPos;

    [Header("UI Elements")]
    public Text droneAmountText;
    public Text roverAmountText;
    public Text droneCompletionPercent;
    public Text roverCompletionPercent;
    public Image droneCompletionBar;
    public Image roverCompletionBar;

    [Header("Settings")]
    public float droneScrapsNeeded = 100f;
    public float droneScrapsContributed;
    public float roverScrapsNeeded = 300f;
    public float roverScrapsContributed;
    int droneAmountToContribute = 0;
    int roverAmountToContribute = 0;

    // Colors
    Color green = Color.green;
    Color yellow = Color.yellow;
    Color red = Color.red;

    public void AddScrapsToDrone()
    {
        droneAmountToContribute += 20;

        if (droneAmountToContribute == 0)
        {
            droneAmountText.text = "+ " + droneAmountToContribute;
            droneAmountText.color = yellow;
        }
        else if (droneAmountToContribute < 0)
        {
            droneAmountText.text = "- " + droneAmountToContribute;
            droneAmountText.color = red;
        }
        else if (droneAmountToContribute > 0)
        {
            droneAmountText.text = "+ " + droneAmountToContribute;
            droneAmountText.color = green;
        }
    }

    public void AddScrapsToRover()
    {
        roverAmountToContribute += 20;

        if (roverAmountToContribute == 0)
        {
            roverAmountText.text = "+ " + roverAmountToContribute;
            roverAmountText.color = yellow;
        }
        else if (roverAmountToContribute < 0)
        {
            roverAmountText.text = "- " + roverAmountToContribute;
            roverAmountText.color = red;
        }
        else if (roverAmountToContribute > 0)
        {
            roverAmountText.text = "+ " + roverAmountToContribute;
            roverAmountText.color = green;
        }
    }

    public void RemoveScrapsFromDrone()
    {
        droneAmountToContribute -= 20;

        if (droneAmountToContribute == 0)
        {
            droneAmountText.text = "+ " + droneAmountToContribute;
            droneAmountText.color = yellow;
        }
        else if (droneAmountToContribute < 0)
        {
            droneAmountText.text = "- " + droneAmountToContribute;
            droneAmountText.color = red;
        }
        else if (droneAmountToContribute > 0)
        {
            droneAmountText.text = "+ " + droneAmountToContribute;
            droneAmountText.color = green;
        }
    }

    public void RemoveScrapsFromRover()
    {
        roverAmountToContribute -= 20;

        if (roverAmountToContribute == 0)
        {
            roverAmountText.text = "+ " + roverAmountToContribute;
            roverAmountText.color = yellow;
        }
        else if (roverAmountToContribute < 0)
        {
            roverAmountText.text = "- " + roverAmountToContribute;
            roverAmountText.color = red;
        }
        else if (roverAmountToContribute > 0)
        {
            roverAmountText.text = "+ " + roverAmountToContribute;
            roverAmountText.color = green;
        }
    }

    public void DroneContribute()
    {
        if (droneAmountToContribute <= Inventory.instance.numberOfScraps)
        {
            if (droneAmountToContribute < 0 && droneScrapsContributed <= 0)
            {
                return;
            }

            AddOrRemoveDroneScraps();
            HandleDroneUI();

            Inventory.instance.CheckAmountOfScraps();

            if (droneScrapsContributed >= droneScrapsNeeded)
            {
                GameObject MiningDrone = Instantiate(dronePrefab, robotSpawnPos.position, Quaternion.identity);
            }
        }
        else
        {
            // Throw error NYI
        }
    }

    public void RoverContribute()
    {
        if (roverAmountToContribute <= Inventory.instance.numberOfScraps)
        {
            if (roverAmountToContribute < 0 && roverScrapsContributed <= 0)
            {
                return;
            }

            AddOrRemoveRoverScraps();
            HandleRoverUI();

            Inventory.instance.CheckAmountOfScraps();

            if (roverScrapsContributed >= roverScrapsNeeded)
            {
                GameObject DuneBuggy = Instantiate(roverPrefab, robotSpawnPos.position, Quaternion.identity);
            }
        }
        else
        {
            // Throw error NYI
        }
    }

    void AddOrRemoveDroneScraps()
    {
        Inventory.instance.numberOfScraps -= droneAmountToContribute;
        droneScrapsContributed += droneAmountToContribute;
        droneAmountToContribute = 0;
    }

    void AddOrRemoveRoverScraps()
    {
        Inventory.instance.numberOfScraps -= roverAmountToContribute;
        roverScrapsContributed += roverAmountToContribute;
        roverAmountToContribute = 0;
    }

    void HandleDroneUI()
    {
        droneAmountText.text = "+ " + droneAmountToContribute;
        droneAmountText.color = yellow;

        droneCompletionBar.fillAmount = droneScrapsContributed / droneScrapsNeeded;
        droneCompletionPercent.text = ((droneScrapsContributed / droneScrapsNeeded) * 100).ToString() + "%";
    }

    void HandleRoverUI()
    {
        roverAmountText.text = "+ " + roverAmountToContribute;
        roverAmountText.color = yellow;

        roverCompletionBar.fillAmount = roverScrapsContributed / roverScrapsNeeded;
        roverCompletionPercent.text = ((roverScrapsContributed / roverScrapsNeeded) * 100).ToString() + "%";
    }

    public void ResetUI()
    {
        droneAmountToContribute = 0;
        droneAmountText.text = "+ " + droneAmountToContribute;
        droneAmountText.color = yellow;
        roverAmountText.text = "+ " + droneAmountToContribute;
        roverAmountText.color = yellow;
    }
}
