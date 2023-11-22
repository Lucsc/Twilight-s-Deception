using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Houses : MonoBehaviour
{
    [SerializeField]
    private Transform destination;

    public bool isOpen;
    public bool isLocked = false;
    public int keyID;
    

    public Transform GetDestination()
    { return destination; }
}
