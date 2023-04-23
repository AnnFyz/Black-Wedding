using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfInteraction
{
    dialog,
    interObj
}
public class InteractionHandler : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    [SerializeField] Transform uiPanel;
    public QuestSO quest;
    public TypeOfInteraction currentTypeOfInteraction;
    private void Start()
    {
        Time.timeScale = 1;
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(false);
            uiPanel.gameObject.SetActive(false);
        }
    }

    void ShowUIPanel()
    {
        if (interactionSymb.gameObject.activeSelf)
        {
            uiPanel.gameObject.SetActive(true);
            interactionSymb.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            uiPanel.gameObject.SetActive(false);
            interactionSymb.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }

    void Perform()
    {
        if (uiPanel.gameObject.activeSelf)
        {
            if (currentTypeOfInteraction == TypeOfInteraction.interObj)
            {
                quest.PerformTask();
            }
        }
    }
   
    void RevealAnswerOptions()
    {
        if (uiPanel.gameObject.activeSelf)
        {
            if (currentTypeOfInteraction == TypeOfInteraction.dialog && quest.isQuestCompleted)
            {
                GetComponent<DialogTree>().RevealSecretAnswer();
            }
        }
    }
    public void CloseUIPanel()
    {
        uiPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
        interactionSymb.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowUIPanel();
            Perform();
            RevealAnswerOptions();
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUIPanel();
        }
    }

}
