using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the sphere's movement
    private Vector3 direction; // Current movement direction
    private Rigidbody rb;
    public bool isInfected;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        ChooseRandomDirection(); // Choose a random initial direction
    }

    private void Update()
    {
        // Move the sphere in the current direction
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reflect the direction upon collision with the wall
        foreach (ContactPoint contact in collision.contacts)
        {
            // Calculate the reflected direction based on the collision normal
            direction = Vector3.Reflect(direction, contact.normal).normalized;
            break; // Only reflect based on the first contact point
        }
    }

    private void ChooseRandomDirection()
    {
        // Choose a random direction for the sphere to start moving in
        direction = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-1f, 1f)).normalized;
    }
    public void SetColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (isInfected)
        {
            renderer.material.color = Color.red;
        }
        else
        {
            renderer.material.color = Color.green;
        }
    }
}
