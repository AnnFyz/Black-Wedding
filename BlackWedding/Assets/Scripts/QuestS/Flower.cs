using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    ObjectInteraction objectInteraction;
    private void Awake()
    {
        objectInteraction = GetComponent<ObjectInteraction>();
        objectInteraction.isQuestObj = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby)
        {
            PickFlower();
        }
    }
    void PickFlower()
    {
        if (objectInteraction.player != null)
        {
            for (int i = 0; i < objectInteraction.player.transform.GetChild(0).childCount; i++) // Flowers child ob has to be the first!
            {
                if (!objectInteraction.player.transform.GetChild(0).GetChild(i).gameObject.activeSelf)
                {
                    objectInteraction.player.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
                    Perform();
                    Destroy(transform.parent.gameObject);
                    break;
                }
             
            }
        }
    }

    void Perform()
    {
        if (objectInteraction.isQuestObj)
        {
           objectInteraction.quest.PerformQuestObjTask();
        }
    }
}
