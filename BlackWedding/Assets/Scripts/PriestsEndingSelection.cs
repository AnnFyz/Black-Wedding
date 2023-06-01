using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriestsEndingSelection : MonoBehaviour
{
    public Action OnOpenedUIPanel;
    public NPCTitle titleOfNPC;
    public BasicInkExample ink;

    void Start()
    {
        if (titleOfNPC == NPCTitle.priest)
        {
            UnsubscribeStartStory();
            OnOpenedUIPanel += LoadPriestEndingStory;
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
                    OnOpenedUIPanel += LoadPriestEndingStory;
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
                    OnOpenedUIPanel += LoadPriestEndingStory;
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
                OnOpenedUIPanel += ink.StartSelectedEndingStory;
                EndingManager.Instance.OnChangedEnding += ink.SelectEndingStory;
                Debug.Log("Load priest ending story");
            }
        }
    }

    void UnsubscribeStartStory()
    {
        if (EndingManager.Instance != null)
        {
            if (titleOfNPC == NPCTitle.priest)
            {
                OnOpenedUIPanel -= ink.StartStory;
                Debug.Log("Unsubscribe start story");
            }
        }
    }
}
