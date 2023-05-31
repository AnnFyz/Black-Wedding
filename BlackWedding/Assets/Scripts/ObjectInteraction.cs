using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    public Transform uiPanel;
    public QuestSO quest;
    public bool isPlayerNearby = false;
    public bool wasTaskPerformed = false;
    public PlayerController player;
    public bool isQuestObj = false;
    public bool isSecondQuestObj = false;
    public bool isInteractable = false;
    public Action OnOpenedUIPanel;


    private void Start()
    {
        QuestManager.Instance.OnUpdatedQuest += UpdateRefToQuest;
        quest = QuestManager.Instance.currentQuest;
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
        quest.OnGivenQuest += MakeQuestObjInteractable;
        quest.OnSecondGivenQuest += MakeSecondQuestObjInteractable;
        if (!isQuestObj && !isSecondQuestObj)
        {
            isInteractable = true;
        }
    }
    

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby && isInteractable && !isQuestObj && !isSecondQuestObj)
        {

            ShowUIPanel();
            if (!wasTaskPerformed)
            {
                Perform();
                wasTaskPerformed = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PlayerController.IsPaused)
            {
                CloseUIPanel();
            }
            else
            {
                uiPanel.gameObject.SetActive(false);
                interactionSymb.gameObject.SetActive(false);
            }
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
            Debug.Log("OPEN PANEL");
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


    void Perform()
    {
        if (!isQuestObj && !isSecondQuestObj)
        {
            if (uiPanel.gameObject.activeSelf)
            {

                quest.PerformObjTask();
            }
        }
    }

    void MakeQuestObjInteractable()
    {
        if (!isSecondQuestObj)
        {
            isInteractable = true;
        }
    }

    void MakeSecondQuestObjInteractable()
    {
        Debug.Log("MakeSecondQuestObjInteractable");
        isInteractable = true;
    }

    void UpdateRefToQuest ()
    {
        quest = QuestManager.Instance.currentQuest;
        quest.OnSecondGivenQuest += MakeSecondQuestObjInteractable;
    }
}
