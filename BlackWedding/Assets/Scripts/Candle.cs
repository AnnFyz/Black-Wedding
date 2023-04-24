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

    [SerializeField] Transform interactionSymb;
    public QuestSO quest;
    [SerializeField] CandleState currentState;
    [SerializeField] float timeToRandomExtinguish = 5f;
    [SerializeField] float maxTimeToRandomExtinguish = 15f;
    [SerializeField] float minTimeToRandomExtinguish = 5f;
    bool isPlayerNearby = false;
    public bool isCoroutineExecuting = false;
    private void Start()
    {
        currentState = (CandleState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CandleState)).Length);
        //currentState = CandleState.lit;
        RandomExtinguish();
        interactionSymb.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerNearby)
        {
            currentState = CandleState.lit;
            timeToRandomExtinguish = UnityEngine.Random.Range(minTimeToRandomExtinguish, maxTimeToRandomExtinguish);
        }

        if (!isCoroutineExecuting)
        {
            RandomExtinguish();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(true);
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionSymb.gameObject.SetActive(false);
            isPlayerNearby = false;
        }
    }

    void RandomExtinguish()
    {
        StartCoroutine(RandomExtinguish(timeToRandomExtinguish));
    }

    IEnumerator RandomExtinguish(float time)
    {
        while (currentState == CandleState.lit && !isCoroutineExecuting)
        {
            isCoroutineExecuting = true;
            new WaitForSeconds(5);
            currentState = (CandleState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CandleState)).Length);
            Debug.Log(currentState);
            yield return new WaitForSeconds(time);
        }
        isCoroutineExecuting = false;
    }

    void Extinguish()
    {

    }
    void Perform()
    {
        quest.PerformTask();
    }
}