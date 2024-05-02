using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Button playButton;
    public Button exitButton; // Reference to the exit button

    void Start()
    {
        playButton.onClick.AddListener(SwitchScene);
        exitButton.onClick.AddListener(ExitGame); // Assign the ExitGame method to the onClick event of the exit button
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("Map1"); // Load the scene with the given name
    }

    public void ExitGame()
    {
        // Call the appropriate method to exit the game depending on the platform
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop playing the game in the Unity Editor
        #else
            Application.Quit(); // Quit the game when built
        #endif
    }
}
