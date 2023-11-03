using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueTree dialogueTree;

    public void TriggerDialogueTree()
    {
        FindObjectOfType<DialogueTreeManager>().StartDialogueTree(dialogueTree);

    }
}
