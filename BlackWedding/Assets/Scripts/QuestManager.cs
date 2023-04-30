using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestSO[] quest;
    public int currentAmountOfInteraction = 0;
    private void Awake()
    {
        Instance = this;
    }

   
}
