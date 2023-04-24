using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionSymb;
    [SerializeField] Transform uiPanel;
    public QuestSO quest;
    public Vector3 defaultAngle;
    bool isPlayerNearby = false;
    private void Start()
    {
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
            uiPanel.gameObject.SetActive(true);
            interactionSymb.gameObject.SetActive(false);
            Time.timeScale = 0;
            PlayerController.IsPaused = true;
        }
        else
        {
            uiPanel.gameObject.SetActive(false);
            interactionSymb.gameObject.SetActive(true);
            Time.timeScale = 1;
            PlayerController.IsPaused = false;
        }
    }

    public void CloseUIPanel()
    {
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

    void RotateToDeafaultAngle()
    {

    }


}
