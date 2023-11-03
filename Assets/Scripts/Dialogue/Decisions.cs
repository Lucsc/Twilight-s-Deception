using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu]
public class Decisions : ScriptableObject
{
    [SerializeField]
    public List<int> playerDecisions = new List<int>();

    private void OnDisable()
    {
        playerDecisions.Clear();
    }
}
