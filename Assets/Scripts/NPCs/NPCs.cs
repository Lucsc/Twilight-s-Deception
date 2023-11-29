using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCs : MonoBehaviour
{
    public GameObject UI;
    public DialogueTrigger trigger;
    private bool inRange;

    [SerializeField]
    private Sprite[] sprites;
    const float MinCycleDelay = 5f;
    float MaxCycleDelay = 10f;
    int currentSpriteIndex = 0;

    private void Start()
    {
        // Idle movement method
        Invoke("CycleSprite", Random.Range(MinCycleDelay, MaxCycleDelay));
        // Set name tag
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("In Range");
            inRange = true;
            collision.GetComponent<Movement>().SetInteractPromptEnabled(true);
            if (sprites.Length != 0)
                SetSprite(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Movement>().SetInteractPromptEnabled(false);
            inRange = false;
        }
    }

    private void Update()
    {
        if(inRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                trigger.TriggerDialogueTree();
            }
        }
    }

    private void CycleSprite()
    {
        if (sprites.Length != 0)
        {
            Invoke("CycleSprite", Random.Range(MinCycleDelay, MaxCycleDelay));
            // Don't cycle when the player is near
            if (inRange)
                return;

            int newSpriteIndex;
            while ((newSpriteIndex = Random.Range(0, sprites.Length)) == currentSpriteIndex) { }

            SetSprite(newSpriteIndex);
        }

    }

    private void SetSprite(int index)
    {
        currentSpriteIndex = index;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}
