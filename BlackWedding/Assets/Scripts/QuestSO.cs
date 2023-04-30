using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TypeOfQuest
{
    GatheringInformation,
    LightCandles,
    GatherFlowers,
    FindingAndReturningOfTornPage,
    ChangeFrescoes
}

[CreateAssetMenu(menuName = "ScriptableObjects/Quests")]
public class QuestSO : ScriptableObject
{
    [SerializeField] TypeOfQuest currentTypeOfQuest;
    public bool isQuestCompleted = false;
    public int amountOfAllWithObjInteractions = 3;
    public int amountOfAllWithNPCInteractions = 3;
    public int startAmountOfAllObjTasks = 3;
    public int startAmountOfAllNPCTasks = 3;
    public int currentAmountOfPerformedObjTasks = 0;
    public int currentAmountOfPerformedNPCTasks = 0;
    public Action OnCompeletedQuest;

    private void OnEnable()
    {
        isQuestCompleted = false;
        amountOfAllWithObjInteractions = startAmountOfAllObjTasks;
        amountOfAllWithNPCInteractions = startAmountOfAllNPCTasks;
        currentAmountOfPerformedObjTasks = 0;
        currentAmountOfPerformedNPCTasks = 0;
    }
    public void PerformObjTask()
    {
        currentAmountOfPerformedObjTasks++;
        Debug.Log("interaction with obj is performed!");
        if (currentAmountOfPerformedObjTasks == amountOfAllWithObjInteractions && currentAmountOfPerformedNPCTasks == amountOfAllWithNPCInteractions)
        {
            isQuestCompleted = true;
            OnCompeletedQuest?.Invoke();
            Debug.Log("quest is completed!");
        }
    }

    public void PerformNPCTask()
    {
        currentAmountOfPerformedNPCTasks++;
        Debug.Log("interaction with NPC is performed!");
        if (currentAmountOfPerformedObjTasks == amountOfAllWithObjInteractions && currentAmountOfPerformedNPCTasks == amountOfAllWithNPCInteractions)
        {
            isQuestCompleted = true;
            OnCompeletedQuest?.Invoke();
            Debug.Log("quest is completed!");
        }
    }
}
