using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
    // Public variable to assign the target GameObject in the Unity Editor
    public GameObject targetSprite;

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
                // Destroy this GameObject
                Destroy(gameObject);
            }
        }
    }
}
