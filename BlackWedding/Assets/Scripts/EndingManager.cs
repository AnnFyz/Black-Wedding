using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Endings
{
    bad,
    neutral,
    good
}
public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }
    public bool SpokeWithPainter = false;
    public bool SpokeWithMother = false;
    public bool SpokeWithPriest = false;
    public Endings currentEnding;
    public Action OnChangedEnding;
    public int endingIndex;

    private void Awake()
    {
        Instance = this;
    }

    public void MarkCompletedConversation(NPCTitle title)
    {
        if (QuestManager.Instance.currentQuest.isQuestCompleted)
        {
            if (title == NPCTitle.painter)
            {
                SpokeWithPainter = true;
            }
            if (title == NPCTitle.mother)
            {
                SpokeWithMother = true;
            }
            if (title == NPCTitle.priest)
            {
                SpokeWithPriest = true;
            }
        }
    }

    public void DetermineTheEnding()
    {
        if (QuestManager.Instance.currentQuest.isQuestCompleted)
        {
            if (SpokeWithPainter)
            {
                currentEnding = Endings.good;
                OnChangedEnding?.Invoke();
            }
            else if (SpokeWithMother)
            {
                currentEnding = Endings.neutral;
                OnChangedEnding?.Invoke();
            }
            else
            {
                currentEnding = Endings.bad;
                OnChangedEnding?.Invoke();
            }
        }
        else
        {
            currentEnding = Endings.bad;
            OnChangedEnding?.Invoke();
        }
    }
}
