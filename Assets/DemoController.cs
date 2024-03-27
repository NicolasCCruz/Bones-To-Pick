using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this to use SceneManager

public class DemoController : MonoBehaviour
{
    public string sceneToLoad; // Assign this in the Unity Editor

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // If Enter key is pressed
        {
            SceneManager.LoadScene(sceneToLoad); // Load the scene
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // If Esc key is pressed
        {
            Application.Quit(); // Quit the game
#if UNITY_EDITOR
            // If running in the Unity editor, stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}