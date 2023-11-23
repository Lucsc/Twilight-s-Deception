using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigItems : MonoBehaviour
{
    private bool canDig;
    public Item item;

    private void Start()
    {
        canDig = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canDig && GameManager.instance.hasShovel)
        {
            Inventory.instance.AddItem(item, 1);
            GameManager.instance.inventoryItems.Add(item);
            Decisions.instance.playerDecisions.Add(item.ID);
            gameObject.SetActive(false);
            AudioManager.instance.Play("Dig");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canDig = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canDig = false;
        }
    }
}
