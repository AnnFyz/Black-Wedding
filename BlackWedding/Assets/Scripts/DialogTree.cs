using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogTree : MonoBehaviour
{
    [SerializeField] GameObject[] answers;
    [SerializeField] TMP_Text NPCText;
    [SerializeField] string newNPCText;
    [SerializeField] int indexOfRevealedAnswer;
    public bool isCoroutineExecuting = false;
    InteractionHandler UIhandler;
    bool isItTimeForResponse = false;
    private void Start()
    {
        answers[indexOfRevealedAnswer].gameObject.SetActive(false);
        UIhandler = GetComponent<InteractionHandler>();
    }
    public void RevealSecretAnswer()
    {
        if (!isItTimeForResponse)
        {
            answers[indexOfRevealedAnswer].gameObject.SetActive(true);
        }
        

    }

    public void NPCResponse()
    {
        NPCText.text = newNPCText;
        isItTimeForResponse = true;
        Time.timeScale = 1f;
        isCoroutineExecuting = false;
        if (isItTimeForResponse)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].SetActive(false);
            }

            StartCoroutine(ExecuteAfterTime(1f));
        }
       
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        Time.timeScale = 1f;
        while (!isCoroutineExecuting)
        {
            isCoroutineExecuting = true;
            yield return new WaitForSeconds(time);
            UIhandler.CloseUIPanel();
            isCoroutineExecuting = false;
        }      

    }
}
