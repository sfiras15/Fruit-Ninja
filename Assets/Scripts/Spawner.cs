using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Reference to the BoxCollider component on the GameObject
    BoxCollider collider;
    // Minimum bounds of the spawn area
    Vector3 minBounds;
    // Maximum bounds of the spawn area
    Vector3 maxBounds;
    // Array of GameObjects that can be spawned
    public GameObject[] fruits;
    // How long the spawned objects will exist before being destroyed
    float maxLifeTime = 5f;
    // Spawning parameters
    float minSpawnTime = 0.5f;
    float maxSpawnTime = 1f;

    float minAngle = -15f;
    float maxAngle = 15f;

    float minForce = 18f;
    float maxForce = 25f;

    public GameObject bombPrefab;



    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        // Set the minimum and maximum bounds of the spawn area
        minBounds = collider.bounds.min;
        maxBounds = collider.bounds.max;
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(Spawn));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    // Coroutine that will keep running while the script is enabled
    private IEnumerator Spawn()
    {
        while (enabled)
        {
            float bombSpawnChance = Random.Range(0f,1f);
            // Generate a random spawn position within the bounds of the BoxCollider
            Vector3 spawnPosition = new Vector3(Random.Range(minBounds.x, maxBounds.x),
                Random.Range(minBounds.y, maxBounds.y), Random.Range(minBounds.z, maxBounds.z));
            // Generate a random rotation for the spawned object
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
            // 10% chance to spawn A bomb otherwise spawn a random Fruit
            if (bombSpawnChance > 0.9f)
            {
                GameObject bomb = Instantiate(bombPrefab, spawnPosition, rotation);
                Destroy(bomb, maxLifeTime);
                // Apply force to the rigidbody of the bomb in the upward direction with a random force between minForce and maxForce
                bomb.GetComponent<Rigidbody>().AddForce(bomb.transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
            }
            else
            {
                int i = Random.Range(0, fruits.Length);
                GameObject fruit = Instantiate(fruits[i], spawnPosition, rotation);
                Destroy(fruit, maxLifeTime);
                // Apply force to the rigidbody of the fruit in the upward direction with a random force between minForce and maxForce
                fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            }
            
        }
        
    }
}
