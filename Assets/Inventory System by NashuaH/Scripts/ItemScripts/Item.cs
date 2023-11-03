
using UnityEngine;


// BASE ITEM
public class Item : ScriptableObject
{
    public string itemName;

    // The ID of every Item needs to be different in order to be saved and loaded 
    public int ID;

    // If you want an item to be stackable, set this bool True
    public bool Stackable;

    // The UI icon of the item 
    public Sprite itemIcon;

    // Description of the item
    public string description;
    

}
