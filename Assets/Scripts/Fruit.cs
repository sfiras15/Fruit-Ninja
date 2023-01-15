using System.Collections;
using System.Collections.Generic;
using UnityEditor.Performance.ProfileAnalyzer;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    // The whole fruit game object
    public GameObject wholeFruit;
    // The sliced fruit game object
    public GameObject slicedFruit;
    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;

    // The particle system for the fruit
    public ParticleSystem juice;
    // The score the player will receive for slicing this fruit
    public int fruitScore = 1;
    private void Awake()
    {
        // Assign the Rigidbody and Collider component to the corresponding variables
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the Blade component from the colliding object
            Blade blade = other.GetComponent<Blade>();
            // Call the Slice function with the blade's cut direction, position, and slice force
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
            // Increase the player's score
            FindObjectOfType<GameManager>().IncreaseScore(fruitScore);
        }
    }
    // Function that handles slicing the fruit
    private void Slice(Vector3 direction, Vector3 spawnPosition,float force)
    {
        // Disable the collider so that the fruit can't be sliced multiple times
        fruitCollider.enabled = false;
        wholeFruit.SetActive(false); 
        slicedFruit.SetActive(true);
        // Play the juice particle system
        juice.Play();
        // Calculate the angle of the slice based on the direction of the blade
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        // Rotate the sliced fruit game object to match the angle of the slice
        slicedFruit.transform.eulerAngles = new Vector3(0f, 0f, angle);
        // Get all the Rigidbody components of the children of the sliced fruit game object
        Rigidbody[] sliced = slicedFruit.GetComponentsInChildren<Rigidbody>();
        // Iterate through each slice and apply force and velocity to it
        foreach (Rigidbody slice in sliced)
        {
            // Set the velocity of the slice to match that of the whole fruit
            slice.velocity = fruitRigidbody.velocity;
            // Apply force to the slice at the position of the blade
            slice.AddForceAtPosition(direction * force, spawnPosition,ForceMode.Impulse);
        }
    }
}
