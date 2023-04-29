using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    [SerializeField] Transform uiPanel;
    public QuestSO quest;
    bool isPlayerNearby = false;
    private void Start()
    {
        uiPanel.gameObject.SetActive(false);
        interactionSymb.gameObject.SetActive(false);
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
        if (uiPanel.gameObject.activeSelf)
        {
    
          quest.PerformTask();
        }
    }
}
