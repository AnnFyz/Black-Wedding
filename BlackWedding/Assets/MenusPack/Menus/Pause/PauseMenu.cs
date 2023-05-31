using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;
    public SceneFader sceneFader;
    public int mainMenuIndex = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
            
        }
       // PauseAllActions();
    }

    void PauseAllActions()
    {
        if (ui.activeSelf)
        {
            PlayerController.IsPaused = true;
            Time.timeScale = 0f;
          
        }
        else
        {
            PlayerController.IsPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);
        PlayerController.IsPaused = ui.activeSelf;
       // PauseAllActions();
    }

    public void Retry()
    {
        Toggle();
        sceneFader.FadeTo((SceneManager.GetActiveScene().buildIndex));
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(mainMenuIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
