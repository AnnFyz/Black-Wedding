using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool questIsPerformed = false;
    public int amountOfAllTasks = 3;
    public int currentAmountOfPerformedTasks = 0;

    public void PerformTask()
    {
        currentAmountOfPerformedTasks++;
        Debug.Log("task is performed!");
        if (currentAmountOfPerformedTasks == amountOfAllTasks)
        {
            questIsPerformed = true;
            Debug.Log("quest is performed!");
        }
    }
}
