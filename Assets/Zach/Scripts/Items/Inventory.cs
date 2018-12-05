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

    public Canvas inventoryCanvas;
    public Image[] slotImages;
    public SpriteRenderer[] slotSprites;
    public Text numOfScrapsText;
    public GameObject slotHolder;
    [HideInInspector] public int allBagSlots;
    Transform[] slot;
    [HideInInspector] public int numberOfScraps = 0;
    bool openedInventory;
    public bool inventoryIsFull;
    [HideInInspector] public bool itemAdded;
    [HideInInspector] public int slotsUsed;

    void Start()
    {
        DetectInventorySlots();
        numOfScrapsText.text = numberOfScraps.ToString();
    }

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

    public void AddItem(GameObject item)
    {
        for (int i = 0; i < allBagSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().slotEmpty && !itemAdded)
            {
                slot[i].GetComponent<Slot>().item = item;
                slot[i].GetComponent<Slot>().icon = item.GetComponent<Pickups>().icon;
                itemAdded = true;
                slotsUsed++;
                
                if (slotsUsed >= allBagSlots)
                {
                    //inventoryIsFull = true;
                }
                else
                {
                    //inventoryIsFull = false;
                }
            }
        }
    }

    public void RemoveItem(GameObject item)
    {
        for (int i = 0; i < allBagSlots; i++)
        {
            if (!slot[i].GetComponent<Slot>().slotEmpty && itemAdded)
            {
                slot[i].GetComponent<Slot>().item = null;
                slot[i].GetComponent<Slot>().icon = null;
                itemAdded = false;
                slotsUsed--;

                if (slotsUsed >= allBagSlots)
                {
                    //inventoryIsFull = true;
                }
                else
                {
                    //inventoryIsFull = false;
                }
            }
        }
    }

    public void CheckAmountOfScraps()
    {
        numOfScrapsText.text = numberOfScraps.ToString();
    }

    void OpenInventory()
    {
        foreach (var slots in slotImages)
        {
            slots.enabled = true;
        }
        foreach (var sprites in slotSprites)
        {
            sprites.enabled = true;
        }
        inventoryCanvas.enabled = true;
        openedInventory = true;
    }

    public void CloseInventory()
    {
        foreach (var slots in slotImages)
        {
            slots.enabled = false;
        }
        foreach (var sprites in slotSprites)
        {
            sprites.enabled = false;
        }
        inventoryCanvas.enabled = false;
        openedInventory = false;
    }

    void DetectInventorySlots()
    {
        allBagSlots = slotHolder.transform.childCount;
        slot = new Transform[allBagSlots];

        for (int i = 0; i < allBagSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i);
        }
    }
}
