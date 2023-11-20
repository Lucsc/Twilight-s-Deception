using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigItems : MonoBehaviour
{
    private bool canDig;

    private void Start()
    {
        canDig = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canDig && GameManager.instance.hasShovel)
        {
            Inventory.instance.AddItem(gameObject.GetComponent<ItemPrefab>().item, 1);
            GameManager.instance.inventoryItems.Add(gameObject.GetComponent<ItemPrefab>().item);
            Decisions.instance.playerDecisions.Add(gameObject.GetComponent<ItemPrefab>().item.ID);
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
