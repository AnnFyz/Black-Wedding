using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NPCTitle
{
    priest,
    mother,
    painter
}
public class NPCInteraction : MonoBehaviour
{

    [SerializeField] Transform interactionSymb;
    [SerializeField] Transform uiPanel;
    public QuestSO quest;
    public Vector3 defaultAngle;
    public bool isPlayerNearby { get; private set; }
    bool wasTaskPerformed = false;
    public Action OnOpenedUIPanel;
    public NPCTitle titleOfNPC;
    public BasicInkExample ink;

    private void Start()
    {
        QuestManager.Instance.currentQuest.OnCompeletedQuest += LoadNewStoryWhileNPCCanvasInactive;
        OnOpenedUIPanel += StartTheStoryWhileNPCCanvasInactive;
        QuestManager.Instance.OnUpdatedQuest += UpdateRefToQuest;
        quest = QuestManager.Instance.currentQuest;
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
        defaultAngle = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby)
            {
                GameManager.Instance.isSpeaking = true;
                ShowUIPanel();
            }
        }
        if (GameManager.Instance.isSpeaking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.isSpeaking = false;
                CloseUIPanel();

            }
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(true);
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(false);
            uiPanel.gameObject.SetActive(false);
            isPlayerNearby = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //RotateTowardsPlayer(other.transform);
        }
    }

    void ShowUIPanel()
    {
        if (interactionSymb.gameObject.activeSelf)
        {
            SceneFader.Instance.FadeInAgain();
            uiPanel.gameObject.SetActive(true);
            interactionSymb.gameObject.SetActive(false);
            Time.timeScale = 0;
            GameManager.Instance.isGamePaused = true;
            OnOpenedUIPanel?.Invoke();
        }
        else
        {
            SceneFader.Instance.FadeInAgain();
            uiPanel.gameObject.SetActive(false);
            interactionSymb.gameObject.SetActive(true);
            Time.timeScale = 1;
            //GameManager.Instance.isGamePaused = false;
        }
    }

    public void CloseUIPanel()
    {
        SceneFader.Instance.FadeInAgain();
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameManager.Instance.isGamePaused = false;
    }

    void RotateTowardsPlayer(Transform player)
    {
        gameObject.transform.GetChild(0).transform.LookAt(player);
    }


    public void Perform()
    {
        if (!wasTaskPerformed)
        {
            if (uiPanel.gameObject.activeSelf)
            {

                quest.PerformNPCTask();
                wasTaskPerformed = true;
            }
        }
    }

    void UpdateRefToQuest()
    {
        quest = QuestManager.Instance.currentQuest;
    }

    void LoadNewStoryWhileNPCCanvasInactive()
    {
        if (titleOfNPC != NPCTitle.priest)
        {
            ink.LoadNewStory();
        }
    }

    void StartTheStoryWhileNPCCanvasInactive()
    {
        if (titleOfNPC != NPCTitle.priest)
        {
            ink.StartStory();
        }
    }

}


