using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string Name;
    public Texture icon;

    public virtual void Equip()
    {

    }

    public virtual void Use()
    {

    }
}
