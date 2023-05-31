using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoryInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    public Transform uiPanel;
    public QuestSO quest;
    public bool isPlayerNearby = false;
    public bool wasTaskPerformed = false;
    public PlayerController player;
    public bool isQuestObj = true;
    public bool isInteractable = true;
    public Action OnOpenedUIPanel;
    public BlueprintInk ink;

    private void Start()
    {
        quest = QuestManager.Instance.currentQuest;
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby && isInteractable) //&& !isQuestObj)
        {
            SceneFader.Instance.FadeInAgain();
            ShowUIPanel();
            if (!wasTaskPerformed)
            {
                PerformQuestObj();
                wasTaskPerformed = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.isSpeaking = true;
            SceneFader.Instance.FadeInAgain();
            CloseUIPanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isInteractable)
        {
            interactionSymb.gameObject.SetActive(true);
            isPlayerNearby = true;
            player = other.gameObject.GetComponent<PlayerController>();
            OnOpenedUIPanel?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(false);
            uiPanel.gameObject.SetActive(false);
            isPlayerNearby = false;
            player = null;
        }
    }
    void ShowUIPanel()
    {
        if (interactionSymb.gameObject.activeSelf)
        {
            uiPanel.gameObject.SetActive(true);
            interactionSymb.gameObject.SetActive(false);
        }
        else
        {
            uiPanel.gameObject.SetActive(false);
            interactionSymb.gameObject.SetActive(true);
        }
    }

    public void CloseUIPanel()
    {
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(true);
    }


    void PerformQuestObj()
    {
        
            if (uiPanel.gameObject.activeSelf)
            {

                quest.PerformQuestObjTask();
            }

    }



    void LoadNewStoryWhileNPCCanvasInactive()
    {

        ink.LoadNewStory();
        ink.LoadSt();

    }

}
