using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyCandle : MonoBehaviour
{
    public GameObject holyCandle;
    ObjectInteraction objectInteraction;
    public bool wasTaskPerformed = false;
    private void Awake()
    {
        objectInteraction = GetComponent<ObjectInteraction>();
        objectInteraction.isQuestObj = false;
        objectInteraction.isSecondQuestObj = true;
    }
    private void Start()
    {
        holyCandle.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && !wasTaskPerformed)
        {
            Debug.Log("Make lit for holy candle");
            
            objectInteraction.uiPanel.gameObject.SetActive(true);
            GameManager.Instance.isGamePaused = true;
            Perform();
            LitHolyCandle();
            wasTaskPerformed = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby)
        {
            Debug.Log("groom");

            //objectInteraction.uiPanel.gameObject.SetActive(true);
            //PlayerController.IsPaused = true;
         
        }
        else
        {
            //objectInteraction.uiPanel.gameObject.SetActive(false);
            //PlayerController.IsPaused = false;
        }
    }

    void LitHolyCandle()
    {
        holyCandle.SetActive(true);
    }
    void Perform()
    {
        if (!wasTaskPerformed)
        {
            objectInteraction.quest.PerformQuestObjTask();
        }
    }
}
