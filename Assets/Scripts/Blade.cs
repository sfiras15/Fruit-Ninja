using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    // A boolean to check if the blade is currently slicing
    bool slicing;
    // Reference to the Collider component on the GameObject
    Collider collider;
    // Minimum velocity required for the blade to start slicing
    float minSliceVelocity = 0.01f;

    TrailRenderer bladeTrail;
    public Vector3 direction { get; private set; }

    // The force of the slice
    public float sliceForce = 5f;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is either pressed or released otherwise keep slicing

        if (Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if (slicing)
        {
            ContinueSlicing();
        }
    }
    private void OnEnable()
    {
        StopSlicing();
    }
    private void OnDisable()
    {
        StopSlicing();
    }
    private void StartSlicing()
    {
        // Get the position of the mouse in screen space
        Vector3 newPos = Input.mousePosition;
        // Convert the mouse position to world space
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        transform.position = newPos;
        slicing = true;
        collider.enabled = true;
        // To avoid inconsistencies when slicing
        bladeTrail.Clear();
    }
    private void StopSlicing()
    {
        slicing = false;
        collider.enabled = false;
    }
    private void ContinueSlicing()
    {
        Vector3 newPos = Input.mousePosition;
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        newPos.z = 0f;
        // Get the direction of the mouse movement
        direction = newPos - transform.position;
        // Calculate the velocity of the mouse movement
        float velocity = direction.magnitude / Time.deltaTime;
        // Enable the Collider component if the velocity is above the "minSliceVelocity" threshold
        collider.enabled = velocity > minSliceVelocity;
        transform.position = newPos;
    }
}
