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
            slotEmpty = false;
            icon = item.GetComponent<Pickups>().icon;
            GetComponent<SpriteRenderer>().sprite = icon;
        }
        else
        {
            slotEmpty = true;
            icon = null;
            GetComponent<SpriteRenderer>().sprite = icon;
        }
    }
}
