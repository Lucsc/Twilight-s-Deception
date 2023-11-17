using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using static System.Collections.Specialized.BitVector32;
using System.Linq;

public class DialogueTreeManager : MonoBehaviour
{
    private Queue<string> sentences;
    public bool isInDialogue;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public TMP_Text[] buttonTexts;

    public Button[] buttons;
    private Button endButton;

    public Animator animator;

    private int currentBranchId = 0;

    public Timer timer;

    private DialogueTree currentDialogue;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        endButton = GameObject.Find("EndButton").GetComponent<Button>();
    }

    public void StartDialogueTree(DialogueTree dialogue)
    {
        currentDialogue = dialogue;
        isInDialogue = true;
        currentBranchId = 0;
        animator.SetBool("IsOpen", true);
        string name = dialogue.NPCName;
        nameText.text = name;
        timer.PauseTimer();
        endButton.gameObject.SetActive(true);
        DialogueLoop();
    }

    public void DialogueLoop()
    {
        if (currentDialogue.branches[currentBranchId].clueID != 0)
        {
            Decisions.instance.playerDecisions.Add(currentDialogue.branches[currentBranchId].clueID);
        }     
        timer.RemoveTime(currentDialogue.branches[currentBranchId].timePenalty);
        if (currentDialogue.branches[currentBranchId].givesItem) {
            Inventory.instance.AddItem(currentDialogue.branches[currentBranchId].item, 1);
            Decisions.instance.playerDecisions.Add(currentDialogue.branches[currentBranchId].item.ID);
        }
        foreach (DialogueSection section in Array.Find(currentDialogue.branches, item => item.branchID == currentBranchId).sections) 
        {
            //Debug.Log(currentBranchId);
            sentences.Enqueue(section.dialogue);
            DisplayNextSentence();
            if (section.responses.Length > 0)
            {
                List<int> indexesToRemove = new List<int>();
                // Removing unobtainable options
                for (int i = 0; i < section.responses.Length; i++)
                { 
                    for (int j = 0; j < section.responses[i].prerequisite.Length; j++)
                    {
                        if (!Decisions.instance.playerDecisions.Contains(section.responses[i].prerequisite[j]))
                        {
                            indexesToRemove.Add(i);
                        }
                    }

                }
                indexesToRemove.Sort();
                List<int> indexesToRemoveNoDupes = indexesToRemove.Distinct().ToList();
                for (int i = indexesToRemoveNoDupes.Count - 1; i >= 0; i--)
                {
                    Debug.Log("Removing: " + indexesToRemoveNoDupes[i]);
                    section.RemoveFromArray(indexesToRemoveNoDupes[i]);
                }
                indexesToRemove.Clear();
                // Displaying Options
                for (int i = 0; i < section.responses.Length; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttonTexts[i].text = section.responses[i].responseDialogue;
                    int temp = i;
                    buttons[i].onClick.RemoveAllListeners();
                    buttons[i].onClick.AddListener(() => SetBranchID(section.responses[temp].nexBranchID)) ;
                }
            }
        }      
    }

    public void SetBranchID(int ID)
    {
        currentDialogue.branches[currentBranchId].sections[0].AddBackFromArray();
        currentBranchId = ID;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        DialogueLoop();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        currentDialogue.branches[currentBranchId].sections[0].AddBackFromArray();
        isInDialogue = false;
        currentBranchId = 0;
        sentences.Clear();
        timer.ResumeTimer();
        animator.SetBool("IsOpen", false);
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

}
