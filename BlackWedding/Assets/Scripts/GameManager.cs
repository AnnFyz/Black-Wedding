using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] int nextSceneIndex;
    public float time;
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
}
