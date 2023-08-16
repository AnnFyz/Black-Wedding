using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TypeOfQuest
{
    GatheringInformation,
    LightCandles,
    GatherFlowers,
    LightHolyCandle,
    ChangeFrescoes,
    PaintersObj
}

[CreateAssetMenu(menuName = "ScriptableObjects/Quests")]
public class QuestSO : ScriptableObject
{
    [SerializeField] TypeOfQuest currentTypeOfQuest;
    public bool isQuestCompleted = false;
    public int amountOfAllWithObjInteractions = 3;
    public int amountOfAllWithNPCInteractions = 3;
    public int amountOfAllWithQuestObjInteractions = 3;
    public int startAmountOfAllObjTasks = 3;
    public int startAmountOfAllNPCTasks = 3;
    public int startAmountOfQuestObjNPCTasks = 3;
    public int currentAmountOfPerformedObjTasks = 0;
    public int currentAmountOfPerformedNPCTasks = 0;
    public int currentAmountOfPerformedQuestObjTasks = 0;
    public Action OnCompeletedQuest;
    public Action OnGivenQuest;
    public Action OnSecondGivenQuest;

    private void OnEnable()
    {
        isQuestCompleted = false;
        amountOfAllWithObjInteractions = startAmountOfAllObjTasks;
        amountOfAllWithNPCInteractions = startAmountOfAllNPCTasks;
        amountOfAllWithQuestObjInteractions = startAmountOfQuestObjNPCTasks;
        currentAmountOfPerformedObjTasks = 0;
        currentAmountOfPerformedNPCTasks = 0;
        currentAmountOfPerformedQuestObjTasks = 0;
    }
    public void PerformObjTask()
    {
        currentAmountOfPerformedObjTasks++;
        Debug.Log("interaction with obj is performed!");
        if (currentAmountOfPerformedObjTasks >= amountOfAllWithObjInteractions && currentAmountOfPerformedNPCTasks >= amountOfAllWithNPCInteractions && currentAmountOfPerformedQuestObjTasks >= amountOfAllWithQuestObjInteractions)
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
        if (currentAmountOfPerformedObjTasks >= amountOfAllWithObjInteractions && currentAmountOfPerformedNPCTasks >= amountOfAllWithNPCInteractions && currentAmountOfPerformedQuestObjTasks >= amountOfAllWithQuestObjInteractions)
        {
            isQuestCompleted = true;
            OnCompeletedQuest?.Invoke();
            Debug.Log("quest is completed!");
        }
    }

    public void PerformQuestObjTask()
    {
        currentAmountOfPerformedQuestObjTasks++;
        Debug.Log("interaction with questObj is performed!");
        if (currentAmountOfPerformedObjTasks >= amountOfAllWithObjInteractions && currentAmountOfPerformedNPCTasks >= amountOfAllWithNPCInteractions && currentAmountOfPerformedQuestObjTasks >= amountOfAllWithQuestObjInteractions)
        {
            isQuestCompleted = true;
            OnCompeletedQuest?.Invoke();
            Debug.Log("quest is completed!");
        }
    }

    public void ReversePerformedQuestObjTask()
    {
        currentAmountOfPerformedQuestObjTasks--;
        Debug.Log("interaction with questObj was reversed!");
    }

    public void MakeObjInteractable()
    {
        OnGivenQuest?.Invoke();
    }

    public void MakeSecondObjInteractable()
    {
        OnSecondGivenQuest?.Invoke();
    }
}
