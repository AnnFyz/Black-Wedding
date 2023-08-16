using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintersObj : MonoBehaviour
{
    ObjectInteraction objectInteraction;
    public bool wasTaskPerformed = false;
    public GameObject dialogueCanvas;
    private void Awake()
    {
        objectInteraction = GetComponent<ObjectInteraction>();
        //objectInteraction.isQuestObj = true;
        objectInteraction.isInteractable = true;
    }
    private void Start()
    {
        dialogueCanvas.SetActive(false);
        objectInteraction.isPaintersObj  = true;
    }
    private void Update()
    {
        if (!wasTaskPerformed)
        {
            if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && !wasTaskPerformed)
            {
                Debug.Log("Collect painters obj");
                Perform();
                dialogueCanvas.SetActive(true);
                wasTaskPerformed = true;
            }
        }
      

        if (!GameManager.Instance.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby)
            {
                GameManager.Instance.isSpeaking = true;
                objectInteraction.ShowUIPanel();
            }
        }
        if (GameManager.Instance.isSpeaking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.isSpeaking = false;
                objectInteraction.CloseUIPanel();

            }
        }

    }
    void Perform()
    {
        if (!wasTaskPerformed)
        {
            objectInteraction.quest.PerformQuestObjTask();
            objectInteraction.DeactivateSymbol();
            objectInteraction.isInteractable = false;
        }
    }
}
