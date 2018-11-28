using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region singleton

    public static Inventory instance;

    private void Awake()
    {
        
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    public GameObject inventoryCanvas;
    [HideInInspector]
    public int numberOfScraps = 0;
    bool openedInventory;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.B))
        {
            if (!openedInventory)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    void OpenInventory()
    {
        inventoryCanvas.SetActive(true);
        openedInventory = true;
    }

    public void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        openedInventory = false;
    }
}
