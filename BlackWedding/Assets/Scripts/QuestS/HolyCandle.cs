using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyCandle : MonoBehaviour
{
    public Renderer[] renderers;
    public Material extinguishedMat;
    public Material litMat;
    ObjectInteraction objectInteraction;
    public bool wasTaskPerformed = false;
    private void Awake()
    {
        objectInteraction = GetComponent<ObjectInteraction>();
        objectInteraction.isQuestObj = false;
        objectInteraction.isSecondQuestObj = true;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && !wasTaskPerformed)
        {
            Debug.Log("Make lit for holy candle");
            Perform();
            LitHolyCandle();
            wasTaskPerformed = true;
        }      
    }

    void LitHolyCandle()
    {
        foreach (var candle in renderers)
        {
            candle.material = litMat;
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
