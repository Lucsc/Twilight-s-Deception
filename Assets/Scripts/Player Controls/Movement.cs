using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class Movement : MonoBehaviour
{
    public DialogueTreeManager dialogueTreeManager;

    public Rigidbody2D player;
    public float speed;
    Vector2 movement;

    //public Animator animator;

    private GameObject currentDoor;

    private Item currentItem;
    private GameObject currentItemPrefab;

    public Decisions playerDecisions;

    private void Start()
    {
        AudioManager.instance.Play("Background Music");
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        //animator.SetFloat("Speed", movement.sqrMagnitude);
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentDoor != null && currentDoor.GetComponent<Houses>().isOpen)
            {
                transform.position = currentDoor.GetComponent<Houses>().GetDestination().position;
            }
            else if(currentItem != null)
            {
                Inventory.instance.AddItem(currentItem, 1);
                playerDecisions.playerDecisions.Add(currentItem.ID);
                currentItemPrefab.SetActive(false);
            }
        }
    }
    void FixedUpdate()
    {
        if (!dialogueTreeManager.isInDialogue)
        {
            Vector2 direction = Vector2.up * movement.y + Vector2.right * movement.x;
            player.velocity = direction * speed;
        }
        else
        {
            player.velocity = Vector2.zero;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            currentDoor = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            if(collision.gameObject == currentDoor)
            {
                currentDoor = null;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            currentItemPrefab = collision.gameObject;
            currentItem = collision.gameObject.GetComponent<ItemPrefab>().item;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            if (collision.gameObject.GetComponent<ItemPrefab>().item == currentItem)
            {
                currentItemPrefab = null;
                currentItem = null;
            }
        }
    }
}