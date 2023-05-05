using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestSO[] quests;
    public QuestSO currentQuest;
    public int questIndex = 0;
    public Action OnUpdatedQuest;
    private void Awake()
    {
        Instance = this;
        currentQuest = quests[questIndex];
        quests[questIndex].OnCompeletedQuest += UpdateQuest;
    }

   void UpdateQuest()
    {
        if(quests.Length > 1 && questIndex < quests.Length)
        {
            currentQuest.currentAmountOfPerformedQuestObjTasks = 0;
            questIndex++;
            Debug.Log("Quest was updated");
        }
        currentQuest = quests[questIndex];
        OnUpdatedQuest?.Invoke();
    }
}
