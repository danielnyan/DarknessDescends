using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailstormSpawner : MonoBehaviour
{
    public float baseSpawnInterval = 0.1f;
    private float currentSpawnInterval;
    public GameObject spawnObject;
    public Vector3 topLeftOffset, bottomRightOffset, velocityLower, velocityUpper;

    private void Start()
    {
        currentSpawnInterval = baseSpawnInterval;
    }

    // Update is called once per frame
    private void Update()
    {
        currentSpawnInterval -= Time.deltaTime;
        if (currentSpawnInterval < 0)
        {
            SpawnProjectile();
            currentSpawnInterval += baseSpawnInterval;
        }
    }

    private void SpawnProjectile()
    {
        Vector3 lowerBounds = transform.position + topLeftOffset;
        Vector3 upperBounds = transform.position + bottomRightOffset;
        Vector3 selectedPos = new Vector3(
                Random.Range(lowerBounds.x, upperBounds.x),
                Random.Range(lowerBounds.y, upperBounds.y),
                Random.Range(lowerBounds.z, upperBounds.z)
            );
        GameObject instance = Instantiate(spawnObject, selectedPos, Quaternion.identity);
        instance.GetComponent<Rigidbody2D>().velocity = new Vector2(
            Random.Range(velocityLower.x, velocityUpper.x),
            Random.Range(velocityLower.y, velocityUpper.y));
    }
}
