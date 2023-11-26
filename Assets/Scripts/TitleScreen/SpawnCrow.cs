using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrow : MonoBehaviour
{
    public GameObject crow;
    public float spawnTime = 5f;
    public Transform[] spawnPoints;
    public Transform endPoint;

    void Start()
    {
        AudioManager.instance.Play("Background Music");
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject newCrow = Instantiate(crow, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        newCrow.GetComponent<CrowMovement>().endPoint = endPoint;
    }

    public void Stop()
    {
        CancelInvoke();
    }

    public void AbleToSpawn()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

}
