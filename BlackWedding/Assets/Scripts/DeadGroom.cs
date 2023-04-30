using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGroom : MonoBehaviour
{
    NPCInteraction npcInteraction;

    private void Start()
    {
       npcInteraction = GetComponent<NPCInteraction>();
       npcInteraction.quest = QuestManager.Instance.quests[QuestManager.Instance.quests.Length -1];
       npcInteraction.quest.OnCompeletedQuest += SetVisible;
       gameObject.SetActive(false);
    }

    void SetVisible()
    {
       gameObject.SetActive(true);
    }
}
