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
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
            PlayerController.IsPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            PlayerController.IsPaused = false;
        }
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
