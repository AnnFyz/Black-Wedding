using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfUIPanel
{
    dialog,
    interObj
}
public class InteractionHandler : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    [SerializeField] Transform uiPanel;
    public TypeOfUIPanel currentTypeOfUIPanel;
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
            Time.timeScale = 1;
        }
    }

    public void CloseUIPanel()
    {
        uiPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowUIPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUIPanel();
        }
    }
}
