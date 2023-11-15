using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;


public class Journal : MonoBehaviour
{
    [Serializable]
    public struct Entries
    {
        public int clueID;
        public string journalEntry;
    }

    public Entries[] entries;

    public GameObject journalUI;
    public TMP_Text journalText;
    private int count;
    private string text;

    private void Start()
    {
        count = Decisions.instance.playerDecisions.Count;
        UpdateJournal();
    }

    public void Update()
    {
        if (count != Decisions.instance.playerDecisions.Count)
        {
            UpdateJournal();
            count = Decisions.instance.playerDecisions.Count;
        }
    }

    public void UpdateJournal()
    {
        text = string.Empty;
        foreach(Entries entry in entries)
        {
            if (Decisions.instance.playerDecisions.Contains(entry.clueID))
            {
                text += "-" + entry.journalEntry + "\n";
            }
        }
        journalText.text = text;
    }
}
