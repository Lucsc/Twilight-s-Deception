using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    [SerializeField] float scaleAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.localScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);
        }
    }
}
