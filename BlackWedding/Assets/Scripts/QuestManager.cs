using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public bool[] canRevealNewOptions;
    private void Awake()
    {
        Instance = this;
    }

  
}
