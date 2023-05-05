using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Candle : MonoBehaviour
{
    enum CandleState
    {
        lit,
        extinguished
    }

    [SerializeField] CandleState currentState;
    [SerializeField] float timeToRandomExtinguish = 0f;
    [SerializeField] float maxTimeToRandomExtinguish = 15f;
    [SerializeField] float minTimeToRandomExtinguish = 5f;
    public bool isCoroutineExecuting = false;
    public new Renderer renderer;
    public Material extinguishedMat;
    public Material litMat;
    ObjectInteraction objectInteraction;
    public bool  wasTaskPerformed = false;
    public bool wasCandleQuestPerformed = false;
    public GameObject light;
    private void Awake()
    {
        light.SetActive(false);
        objectInteraction = GetComponent<ObjectInteraction>();
        objectInteraction.isQuestObj = true;
    }
    private void Start()
    {
        objectInteraction.quest.OnCompeletedQuest += CancelExtinguishAfterCompletingQuest;
        currentState = (CandleState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CandleState)).Length);
        RandomExtinguish();
        SetRightMatAndParticles();
        timeToRandomExtinguish = UnityEngine.Random.Range(minTimeToRandomExtinguish, maxTimeToRandomExtinguish);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && currentState != CandleState.lit && !wasTaskPerformed)
        {
            Debug.Log("Make lit");
            currentState = CandleState.lit;
            SetRightMatAndParticles();
            IncreaseTimeForRandomExtinguish();
            Perform();
            wasTaskPerformed = true;
        }

        if (!isCoroutineExecuting && !wasCandleQuestPerformed)
        {
            RandomExtinguish();
        }
    }


    void RandomExtinguish()
    {
        if (!wasCandleQuestPerformed)
        {
            StartCoroutine(RandomExtinguish(timeToRandomExtinguish));
        }
    }

    IEnumerator RandomExtinguish(float time)
    {
        while (currentState == CandleState.lit && !isCoroutineExecuting)
        {
            isCoroutineExecuting = true;
            yield return new WaitForSeconds(time);
            currentState = CandleState.extinguished;
            ReversePerformed();
            Debug.Log(currentState);
            SetRightMatAndParticles();
        }
        isCoroutineExecuting = false;
    }

    void SetRightMatAndParticles()
    {
        if (currentState == CandleState.extinguished)
        {
            light.SetActive(false);
            renderer.material = extinguishedMat;
        }
        if (currentState == CandleState.lit)
        {
            light.SetActive(true);
            renderer.material = litMat;
        }
    }

    void Perform()
    {
        if (!wasTaskPerformed)
        {
            objectInteraction.quest.PerformQuestObjTask();
        }
    }

    void ReversePerformed()
    {
        if (wasTaskPerformed && !wasCandleQuestPerformed)
        {
            objectInteraction.quest.ReversePerformedQuestObjTask();
            wasTaskPerformed = false;
        }
    }
    void IncreaseTimeForRandomExtinguish()
    {
        maxTimeToRandomExtinguish *= 1.2f;
        minTimeToRandomExtinguish *= 1.2f;
        timeToRandomExtinguish = UnityEngine.Random.Range(minTimeToRandomExtinguish, maxTimeToRandomExtinguish);
    }

    void CancelExtinguishAfterCompletingQuest()
    {
        StopAllCoroutines();        wasCandleQuestPerformed = true;
    }
}