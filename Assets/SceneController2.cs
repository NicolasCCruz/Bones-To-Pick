using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Necessary for changing scenes

public class SceneController2 : MonoBehaviour
{
    public GameObject jeanGameObject; // Assign your "Jean 1_0" GameObject in the inspector

    // Update is called once per frame
    void Update()
    {
        // Check if Jean's y position is at or above 4.74
        if(jeanGameObject.transform.position.y >= 4.74f)
        {
            // Change to the scene named "map2"
            SceneManager.LoadScene("map2");
        }
    }
}

