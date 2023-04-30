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
    public int amountOfAllTasks = 3;
    public int startAmountOfAllTasks = 3;
    public int currentAmountOfPerformedTasks = 0;
    public Action OnCompeletedQuest;

    private void OnEnable()
    {
        isQuestCompleted = false;
        amountOfAllTasks = startAmountOfAllTasks;
        currentAmountOfPerformedTasks = 0;
    }
    public void PerformTask()
    {
        currentAmountOfPerformedTasks++;
        Debug.Log("task is performed!");
        if (currentAmountOfPerformedTasks == amountOfAllTasks)
        {
            isQuestCompleted = true;
            OnCompeletedQuest?.Invoke();
            Debug.Log("quest is completed!");
        }
    }
}
