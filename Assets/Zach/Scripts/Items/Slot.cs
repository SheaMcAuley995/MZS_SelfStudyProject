using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    Sprite icon;


    public void addItem(Item item)
    {
        gameObject.name = item.name;
        icon = item.icon;

    }
}
