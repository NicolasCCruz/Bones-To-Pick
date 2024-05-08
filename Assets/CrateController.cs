using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public GameObject holeSprite; // Assign in Unity Editor
    public GameObject wallSprite; // Assign in Unity Editor
    public float speed = 2f; // Speed of the movement
    public AudioClip movingSound; // Assign this sound in Unity Editor

    private Rigidbody2D rb2d;
    private bool hasStartedMovingTowardsHole = false; // New flag to check if the movement has started
    private AudioSource audioSource; // AudioSource component
    private Vector3 lastPosition;
    
    private float soundStopTimer = 0.5f; // 0.5 seconds delay before stopping the sound
    private float timeSinceLastMove = 0f; // Time since the last movement occurred


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        lastPosition = transform.position; // Initialize lastPosition
    }

    // Update is called once per frame
    void Update()
{
    if (IsWithinRange() || hasStartedMovingTowardsHole)
    {
        if (!hasStartedMovingTowardsHole)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            hasStartedMovingTowardsHole = true;
        }

        MoveTowardsHole();
    }

    // Check if position has changed
    if (transform.position != lastPosition)
    {
        PlayMovingSound();
        timeSinceLastMove = 0f; // Reset the timer since the object moved
    }
    else
    {
        if (timeSinceLastMove >= soundStopTimer)
        {
            StopMovingSound();
        }
        else
        {
            timeSinceLastMove += Time.deltaTime; // Update the timer
        }
    }

    lastPosition = transform.position;
}


    bool IsWithinRange()
    {
        if (wallSprite == null || hasStartedMovingTowardsHole) return true; // If already moving, ignore range check

        float distanceToWall = Vector3.Distance(transform.position, wallSprite.transform.position);
        return distanceToWall <= Mathf.Sqrt(1.6f * 1.6f + 0.5f * 0.5f);
    }

    void MoveTowardsHole()
    {
        if (holeSprite == null) return;

        Vector3 positionToMove = new Vector3(holeSprite.transform.position.x, transform.position.y, transform.position.z);

        // Move on X axis
        if (transform.position.x != holeSprite.transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMove, speed * Time.deltaTime);
        }
        // Then, move on Y axis
        else if (transform.position.y != holeSprite.transform.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, holeSprite.transform.position, speed * Time.deltaTime);
        }

        // Check if the Crate has arrived at the holeSprite's position
        if (transform.position == holeSprite.transform.position)
        {
            Destroy(holeSprite); // Delete holeSprite object
        }
    }

    private void PlayMovingSound()
{
    if (!audioSource.isPlaying)
    {
        audioSource.PlayOneShot(movingSound);
    }
}

private void StopMovingSound()
{
    if (audioSource.isPlaying)
    {
        audioSource.Stop();
    }
}
}
