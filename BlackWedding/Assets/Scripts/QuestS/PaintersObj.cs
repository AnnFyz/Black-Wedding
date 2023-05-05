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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && !wasTaskPerformed)
        {
            Debug.Log("Collect painters obj");
            Perform();
            dialogueCanvas.SetActive(true);
            wasTaskPerformed = true;
        }
    }
        void Perform()
    {
        if (!wasTaskPerformed)
        {
            objectInteraction.quest.PerformQuestObjTask();
        }
    }
}
