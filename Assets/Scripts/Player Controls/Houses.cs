using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Houses : MonoBehaviour
{
    [SerializeField]
    private Transform destination;

    public bool isOpen;
    

    public Transform GetDestination()
    { return destination; }
}
