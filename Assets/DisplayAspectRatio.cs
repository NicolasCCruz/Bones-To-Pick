using UnityEngine;

public class DisplayAspectRatio : MonoBehaviour
{
    void Update()
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;
        Debug.Log("Current Aspect Ratio: " + currentAspectRatio.ToString("F2"));
        Debug.Log("Current Resolution: " + "Width: " + Screen.width + "Height: " + Screen.height);

        // Calculate width based on a given height and aspect ratio
        float givenHeight = Screen.height; // example height
        float aspectRatio = 2.25f; // example aspect ratio
        float calculatedWidth = givenHeight * aspectRatio;

        // Log the calculated width based on the given height and aspect ratio
        Debug.Log("Calculated Width from given height " + givenHeight + " with aspect ratio " + aspectRatio + ": " + calculatedWidth);
    }
}