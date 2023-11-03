using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    public int[] prerequisite;
    public int nexBranchID;
    [TextArea(1, 3)]
    public string responseDialogue;
}
[System.Serializable]
public class DialogueSection
{
    public DialogueResponse[] responses;
    private List<DialogueResponse> removedResponses = new List<DialogueResponse>();
    [TextArea(1, 10)]
    public string dialogue;

    public void RemoveFromArray(int indexToRemove)
    {

        if (indexToRemove < 0)
        {
            Debug.Log("Invalid Index: " + indexToRemove);
        }
        else
        {
            var tempArray = new DialogueResponse[responses.Length - 1];
            removedResponses.Add(responses[indexToRemove]);
            Array.Copy(responses, 0, tempArray, 0, indexToRemove);
            Array.Copy(responses, indexToRemove + 1, tempArray, indexToRemove, responses.Length - indexToRemove - 1);

            responses = tempArray;
        }

    }
    public void AddBackFromArray()
    {
        if (removedResponses.Count > 0)
        {
            DialogueResponse[] removedResponsesArray = removedResponses.ToArray();
            removedResponses.Clear();
            var tempArray = responses.Concat(removedResponsesArray).ToArray();

            responses = tempArray;
        }
    }
}
[System.Serializable]
public class DialogueBranch
{
    public string branchName;
    public int branchID;
    public int clueID;
    public int timePenalty;
    public DialogueSection[] sections;
    public bool endOnFinal;
}

public class DialogueTree : MonoBehaviour
{
    public DialogueBranch[] branches;
    public string NPCName;
}
