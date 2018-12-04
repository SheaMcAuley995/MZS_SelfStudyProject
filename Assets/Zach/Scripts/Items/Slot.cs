using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public GameObject item;
    public Sprite icon;
    public bool slotEmpty;

    void Start()
    {
        
    }

    void Update()
    {
        if (item)
        {
            FillSlot();
        }
        else
        {
            RemoveSlot();
        }
    }

    void FillSlot()
    {
        slotEmpty = false;
        icon = item.GetComponent<Pickups>().icon;
        GetComponent<SpriteRenderer>().sprite = icon;
    }

    void RemoveSlot()
    {
        slotEmpty = true;
        icon = null;
        GetComponent<SpriteRenderer>().sprite = icon;
    }
}
