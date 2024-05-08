using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
    // Public variable to assign the target GameObject in the Unity Editor
    public GameObject targetSprite;

    // Public variable to assign the AudioSource component in the Unity Editor
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        // Check if targetSprite has been assigned to prevent null reference exceptions
        if (targetSprite != null)
        {
            // Calculate the distance between the current GameObject and the targetSprite
            float distanceX = Mathf.Abs(transform.position.x - targetSprite.transform.position.x);
            float distanceY = Mathf.Abs(transform.position.y - targetSprite.transform.position.y);

            // Check if both distances are within 1.5 pixels and if the space bar is pressed
            if (distanceX <= 1.5f && distanceY <= 1.5f && Input.GetKeyDown(KeyCode.Space))
            {
                // Play the audio clip if not already playing
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                // Wait for the audio to finish before destroying the GameObject
                StartCoroutine(DestroyAfterSound(audioSource.clip.length));
            }
        }
    }

    // Coroutine to destroy the GameObject after the sound has finished playing
    IEnumerator DestroyAfterSound(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy this GameObject
        Destroy(gameObject);
    }
}
