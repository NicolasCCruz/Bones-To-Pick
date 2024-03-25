using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this to use SceneManager

public class SceneController : MonoBehaviour
{
    public Button playButton;
    void Start()
    {
        playButton.onClick.AddListener(SwitchScene);
    }
    public void SwitchScene()
    {
        SceneManager.LoadScene("Map1"); // Load the scene with the given name
    }
}
