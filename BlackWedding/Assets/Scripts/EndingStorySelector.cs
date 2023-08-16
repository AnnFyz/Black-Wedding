using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingStorySelector : MonoBehaviour
{
   public static EndingStorySelector Instance { get; private set; }
    public int endingStoryIndex = 0;
    public BasicInkExample inkStory;
    public GameObject canvasDialogie;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        canvasDialogie.SetActive(false);
    }
    public void SelectEndingStory(int newIndex)
    {
        endingStoryIndex = newIndex;
        canvasDialogie.SetActive(true);
        inkStory.storyIndex = endingStoryIndex;
        inkStory.StartStory();
        canvasDialogie.SetActive(true);
    }
}
