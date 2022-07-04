using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonScript : MonoBehaviour
{

    public GameObject pauseCanvas;
    private AudioListener listener;

    private void Start()
    {
        listener = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();
    }

    public void StartScene()
    {
        //Debug.Log("Resseting Scene");
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        PlayerController.isPaused = false;
    
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        PlayerController.isPaused = false;
        listener.enabled = true;

    }
}
