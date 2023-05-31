using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] int nextSceneIndex;
    public float time;
    public bool isGamePaused;
    public bool isSpeaking;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneIndex);
        Time.timeScale = 1;
    }

    private void Update()
    {

        time = Time.timeScale;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void PauseAllActions(bool timeToPause)
    {
        if (timeToPause)
        {
            GameManager.Instance.isGamePaused = true;
            Time.timeScale = 0f;

        }
        else
        {
            GameManager.Instance.isGamePaused = false;
            Time.timeScale = 1f;
        }
    }
}
