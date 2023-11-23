using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowMovement : MonoBehaviour
{
    public Transform endPoint;
    public float speed = 1f;

    void Start()
    {
        if (transform.position.x > endPoint.position.x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
        if (transform.position == endPoint.position)
        {
            Destroy(gameObject);
        }
    }
}
