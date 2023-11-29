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
    const float MinMovementTheshold = 0.01f;

    public Animator animator;

    private GameObject currentDoor;

    private Item currentItem;
    private GameObject currentItemPrefab;

    private float nextStep;
    public float stepDelay;

    [SerializeField]
    GameObject InteractPrompt;


    private void Start()
    {
        stepDelay = speed / 9;

        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        bool locked = false;

        SetAnimatorState();

        //animator.SetFloat("Speed", movement.sqrMagnitude);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(currentDoor != null)
            {
                if (currentDoor.GetComponent<Houses>().isLocked)
                {
                    locked = true;
                    Decisions.instance.playerDecisions.ForEach(clue =>
                    {
                        if (clue == currentDoor.GetComponent<Houses>().keyID)
                        {
                            transform.position = currentDoor.GetComponent<Houses>().GetDestination().position;
                            locked = false;
                            AudioManager.instance.Play("Door");
                        }
                    });
                    if (locked)
                    {
                        AudioManager.instance.Play("Locked");
                    }
                } else {
                    AudioManager.instance.Play("Door");
                    transform.position = currentDoor.GetComponent<Houses>().GetDestination().position;
                }
            }
            else if(currentItem != null)
            {
                Inventory.instance.AddItem(currentItem, 1);
                if (currentItem.itemName == "Shovel")
                {
                    GameManager.instance.hasShovel = true;
                }

                currentItemPrefab.SetActive(false);
            }

        }
    }

    private void SetAnimatorState()
    {
        bool idle = Mathf.Abs(movement.x) < MinMovementTheshold && Mathf.Abs(movement.y) < MinMovementTheshold;
        bool up = movement.y > MinMovementTheshold;
        bool down = movement.y < -MinMovementTheshold;
        bool left = movement.x < MinMovementTheshold;
        bool right = movement.x > -MinMovementTheshold;


        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            // More horizontal
            up = down = false;
        }
        else
        {
            right = left = false;
        }

        animator.SetBool("Idle", idle);
        animator.SetBool("Up", up);
        animator.SetBool("Down", down);
        animator.SetBool("Left", left);
        animator.SetBool("Right", right);
    }

    void FixedUpdate()
    {
        if (!dialogueTreeManager.isInDialogue)
        {
            Vector2 direction = Vector2.up * movement.y + Vector2.right * movement.x;
            player.velocity = direction * speed;
            if (player.velocity.sqrMagnitude > 0)
            {
                if (Time.time > nextStep && Time.timeScale != 0.0f)
                {
                    AudioManager.instance.Play("Walking");
                    nextStep = Time.time + stepDelay;
                }

            }
            else
            {
                AudioManager.instance.Stop("Walking");
            }
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
            SetInteractPromptEnabled(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            if(collision.gameObject == currentDoor)
            {
                currentDoor = null;
                SetInteractPromptEnabled(false);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            currentItemPrefab = collision.gameObject;
            currentItem = collision.gameObject.GetComponent<ItemPrefab>().item;
            SetInteractPromptEnabled(true);
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
                SetInteractPromptEnabled(false);
            }
        }
    }

    public void SetInteractPromptEnabled(bool enabled)
    {
        if (InteractPrompt != null)
            InteractPrompt.SetActive(enabled);
    }
}