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
    public enum NPCState
    {
        interactable,
        notInteractable
    }

    [SerializeField] Transform interactionSymb;
    [SerializeField] Transform uiPanel;
    public QuestSO quest;
    public Vector3 defaultAngle;
    bool isPlayerNearby = false;
    bool wasTaskPerformed = false;
    public Action OnOpenedUIPanel;
    public NPCTitle titleOfNPC;
    public BasicInkExample ink;

    private void Start()
    {
        QuestManager.Instance.currentQuest.OnCompeletedQuest += LoadNewStoryWhileNPCCanvasInactive;
        if (titleOfNPC == NPCTitle.priest)
        {
            UnsubscribeStartStory();
            OnOpenedUIPanel += LoadPriestEndingStory;
        }
        // the same updating for ending story
        QuestManager.Instance.OnUpdatedQuest += UpdateRefToQuest;
        quest = QuestManager.Instance.currentQuest;
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
        defaultAngle = transform.rotation.eulerAngles;
    }

    private void Update()
    { 

        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby)
        {

            ShowUIPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUIPanel();
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
            RotateTowardsPlayer(other.transform);
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
            PlayerController.IsPaused = true;
            OnOpenedUIPanel?.Invoke();
        }
        else
        {
            SceneFader.Instance.FadeInAgain();
            uiPanel.gameObject.SetActive(false);
            interactionSymb.gameObject.SetActive(true);
            Time.timeScale = 1;
            PlayerController.IsPaused = false;
        }
    }

    public void CloseUIPanel()
    {
        SceneFader.Instance.FadeInAgain();
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(true);
        Time.timeScale = 1;
        PlayerController.IsPaused = false;
    }

    void RotateTowardsPlayer(Transform player)
    {
        //transform.LookAt(player, Vector3.up); 
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
      
            ink.LoadNewStory();
            ink.LoadSt();
           
    }

    void LoadPriestEndingStory()
    {

        if (titleOfNPC == NPCTitle.priest)
        {
            if (EndingManager.Instance != null)
            {
                OnOpenedUIPanel += ink.StartSelectedEndingStory;
                EndingManager.Instance.OnChangedEnding += ink.SelectEndingStory;
                Debug.Log("Load priest ending story");
            }
        }
    }

    void UnsubscribeStartStory()
    {
        if (EndingManager.Instance != null)
        {
            if (titleOfNPC == NPCTitle.priest)
            {
                OnOpenedUIPanel -= ink.StartStory;
                Debug.Log("Unsubscribe start story");
            }
        }
    }
}
