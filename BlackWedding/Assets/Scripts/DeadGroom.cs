using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGroom : MonoBehaviour
{
    NPCInteraction npcInteraction;
    public GameObject dialogueCan;
    private void Start()
    {
     
       QuestManager.Instance.quests[1].OnCompeletedQuest += SetVisible;
        dialogueCan.SetActive(false);
    }

    void SetVisible()
    {
        if(QuestManager.Instance.questIndex > 0)
        {
            dialogueCan.SetActive(true);
        }
    }


}
