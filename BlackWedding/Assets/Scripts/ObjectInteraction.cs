using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    public Transform uiPanel;
    public QuestSO quest;
    public bool isPlayerNearby = false;
    bool wasTaskPerformed = false;
    public PlayerController player;
    public bool isQuestObj = false;
    public bool isInteractable = false;

    private void Start()
    {
        quest = QuestManager.Instance.currentQuest;
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
        quest.OnGivenQuest += MakeQuestObjInteractable;
        if (!isQuestObj)
        {
            isInteractable = true;
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby && isInteractable)
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


    void Perform()
    {
        if (!isQuestObj)
        {
            if (uiPanel.gameObject.activeSelf)
            {

                quest.PerformObjTask();
            }
        }
    }

    void MakeQuestObjInteractable()
    {
        isInteractable = true;
    }
}
