using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnObject;

    public float frequency;
    public float initialSpeed;

    float lastSpawnTime;

    private void Update()
    {
        if (Time.time > lastSpawnTime + frequency)
        {
            Spawning();
            lastSpawnTime = Time.time;
        }
    }

    public void Spawning()
    {
        GameObject newSpawnedObject = Instantiate(spawnObject, transform.position, Quaternion.identity);
        newSpawnedObject.GetComponent<Rigidbody2D>().velocity = transform.forward * initialSpeed;
        newSpawnedObject.transform.parent = transform;
    }
}
