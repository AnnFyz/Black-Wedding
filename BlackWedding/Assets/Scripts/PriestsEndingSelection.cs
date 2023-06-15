using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriestsEndingSelection : MonoBehaviour
{
    public NPCTitle titleOfNPC;
    public BasicInkExample ink;

    void Start()
    {
        if (titleOfNPC == NPCTitle.priest)
        {
            UnsubscribeStartStory();
            gameObject.GetComponent<NPCInteraction>().OnOpenedUIPanel += LoadPriestEndingStory;
            EndingManager.Instance.OnChangedEnding += ink.SelectEndingStory;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<NPCInteraction>().isPlayerNearby)
            {
                if (titleOfNPC == NPCTitle.priest)
                {

                    ink.LoadNewStory();
                }
            }
        }
        if (GameManager.Instance.isSpeaking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (titleOfNPC == NPCTitle.priest)
                {
                    ink.LoadNewStory();
                }

            }
        }

    }

    void LoadPriestEndingStory()
    {

        if (titleOfNPC == NPCTitle.priest)
        {
            if (EndingManager.Instance != null)
            {
                ink.StartEndingStory();
                Debug.Log("StartSelectedEndingStory");
            }
        }
    }

    void UnsubscribeStartStory()
    {
        if (EndingManager.Instance != null)
        {
            if (titleOfNPC == NPCTitle.priest)
            {
                gameObject.GetComponent<NPCInteraction>().OnOpenedUIPanel -= ink.StartStory;
            }
        }
    }
}
