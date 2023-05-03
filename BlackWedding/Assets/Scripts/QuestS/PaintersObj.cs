using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintersObj : MonoBehaviour
{
    ObjectInteraction objectInteraction;
    public bool wasTaskPerformed = false;
    private void Awake()
    {
        objectInteraction = GetComponent<ObjectInteraction>();
        //objectInteraction.isQuestObj = true;
        objectInteraction.isInteractable = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && !wasTaskPerformed)
        {
            Debug.Log("Collect painters obj");
            Perform();
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
