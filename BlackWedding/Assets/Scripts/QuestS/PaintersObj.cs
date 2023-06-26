using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintersObj : ObjectInteraction
{
    [SerializeField] Transform interactionSymb;
    ObjectInteraction objectInteraction;
    public bool wasPainterTaskPerformed = false;
    public GameObject dialogueCanvas;
    private void Awake()
    {
        objectInteraction = GetComponent<ObjectInteraction>();
        objectInteraction.isInteractable = true;
    }
    private void Start() 
    {
        dialogueCanvas.SetActive(false);
        StartHandler();
    }

    public override void StartHandler()
    {
        base.StartHandler();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby && !wasTaskPerformed)
        {
            Debug.Log("Collect painters obj");
            Perform();
            dialogueCanvas.SetActive(true);
            wasTaskPerformed = true;
        }
        UpdateHandler();
    }

    public override void UpdateHandler()
    {
        if (!GameManager.Instance.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.E) && objectInteraction.isPlayerNearby)
            {
                GameManager.Instance.isSpeaking = true;
                objectInteraction.ShowUIPanel();
            }
        }
        if (GameManager.Instance.isSpeaking)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.isSpeaking = false;
                objectInteraction.CloseUIPanel();

            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    void Perform()
    {
        if (!wasTaskPerformed)
        {
            objectInteraction.quest.PerformQuestObjTask();
            objectInteraction.isInteractable = false;
            objectInteraction.DeactivateSymbol();
        }
    }
}
