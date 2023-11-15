using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemPrefab : MonoBehaviour
{
    public Item item;

    public ItemPrefab(Item item)
    {
        this.item = item;

    }
}
