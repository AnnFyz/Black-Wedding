using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;
	[SerializeField]
	public int storyIndex = 0;
	[SerializeField]
	public NPCInteraction NPC;
	[SerializeField] bool IsThisNPSGivesQuest = false;
	[SerializeField]
	public int storyIndexToActivateQuestObj = 0;
	[SerializeField]
	public int storyIndexToActivateSecondQuestObj = 0;
	[SerializeField] TextAsset[] inkJSONAssetsEndings;
	public int endingStoryIndex; //{ get; private set; }
	public GameObject paintersObj;
	void Awake () {
		// Remove the default message
		RemoveChildren();
		StartStory();
		NPC = GetComponentInParent<NPCInteraction>();
	}

	
    // Creates a new Story object with the compiled story which we can then play!
    public void StartStory () 
	{
		Canvas.ForceUpdateCanvases();
		if (inkJSONAssets.Length > 0 && storyIndex < inkJSONAssets.Length)
        {
			story = new Story(inkJSONAssets[storyIndex].text);
		}
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	public void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();
		// Read all the content until we can't continue any more
		while (story.canContinue)
		{
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);


			if (story.currentTags.Contains("Ending"))
			{
				
					Debug.Log("LoadEndingScene");
					SceneFader.Instance.FadeTo();
					Time.timeScale = 1f;
					if (EndingStorySelector.Instance != null)
					{
						EndingStorySelector.Instance.SelectEndingStory(endingStoryIndex);

					}
			}

			if (story.currentTags.Contains("LoadNewScene"))
			{
				//Button choice = CreateChoiceView("End of story.\nLoad new scene?");
				Button choice = CreateChoiceView("Start new chapter?");
				choice.onClick.AddListener(delegate
				{
					SceneFader.Instance.FadeTo();
					GameManager.Instance.LoadNextScene();
					Time.timeScale = 1f;
					//GameManager.Instance.isGamePaused = false;
				});
			}

		}

		// Display all the choices, if there are any!
		if (story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}

		// If we've read all the content and there's no choices, the story is finished!
		
		else if (story.currentChoices.Count == 0)
		{
			if(EndingManager.Instance != null && NPC != null)
            {
				EndingManager.Instance.MarkCompletedConversation(NPC.titleOfNPC);
				EndingManager.Instance.DetermineTheEnding();

			}
			ActivateQuestObj();
			if(NPC != null)
            {
				NPC.Perform();
				SceneFader.Instance.FadeInAgain();
				NPC.CloseUIPanel();
			}
            else if(paintersObj != null)
            {
				SceneFader.Instance.FadeInAgain();
				paintersObj.SetActive(false);
			}
		}

		//if (story.currentTags.Contains("LoadNewScene"))
		//{
		//	//Button choice = CreateChoiceView("End of story.\nLoad new scene?");
		//	Button choice = CreateChoiceView("Start new chapter?");
		//	choice.onClick.AddListener(delegate
		//	{
		//		SceneFader.Instance.FadeTo();
		//		GameManager.Instance.LoadNextScene();
		//		Time.timeScale = 1f;
		//		//GameManager.Instance.isGamePaused = false;
		//	});
		//}

		if (story.currentTags.Contains("Ending"))
		{
			Button choice = CreateChoiceView("End the story");
			choice.onClick.AddListener(delegate
			{
				Debug.Log("LoadEndingScene");
				SceneFader.Instance.FadeTo();
				Time.timeScale = 1f;
				if(EndingStorySelector.Instance != null)
                {
					EndingStorySelector.Instance.SelectEndingStory(endingStoryIndex);

				}

			});
		}

		Canvas.ForceUpdateCanvases();
	}


	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		if (story.currentTags.Count > 0 && story.currentTags.Contains("Italic"))
		{
			storyText = Instantiate(textPrefabItalic) as TMP_Text;
		}
        else 
        {
			storyText = Instantiate(TextPrefab) as TMP_Text;
		}
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
		canvas.GetComponent<VerticalLayoutGroup>().spacing = 0;
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);

		// Gets the text from the button prefab
		TMP_Text choiceText = choice.GetComponentInChildren<TMP_Text> ();
		//choiceText.text = text;
		choiceText.text = text.Trim();
		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;
		return choice;
	}


	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	public void LoadNewStory()
    {		
		if(inkJSONAssets.Length > 1 && storyIndex < inkJSONAssets.Length -1)
		{
			//Debug.Log("was loaded new Story");
			storyIndex++;
			Canvas.ForceUpdateCanvases();
			StartStory();
		}
    }


	void ActivateQuestObj()
    {
        if (IsThisNPSGivesQuest)
        {
			if(storyIndex == storyIndexToActivateQuestObj && NPC.quest == QuestManager.Instance.quests[0])
            {
				QuestManager.Instance.currentQuest.MakeObjInteractable();
            }

			if (storyIndex == storyIndexToActivateSecondQuestObj && NPC.quest == QuestManager.Instance.quests[QuestManager.Instance.quests.Length - 1])
			{
				Debug.Log("MakeSecondObjInteractable");
				QuestManager.Instance.currentQuest.MakeSecondObjInteractable();
			}
		}
	}

    public void SelectEndingStory()
    {
        if (EndingManager.Instance.currentEnding == Endings.bad)
        {
            endingStoryIndex = 0;
            storyIndex = 0;
			Debug.Log("SelectEndingStory" + endingStoryIndex);

		}
        if (EndingManager.Instance.currentEnding == Endings.neutral)
        {
            endingStoryIndex = 1;
            storyIndex = 1;
			Debug.Log("SelectEndingStory" + endingStoryIndex);

		}
        if (EndingManager.Instance.currentEnding == Endings.good)
        {
            endingStoryIndex = 2;
            storyIndex = 2;
			Debug.Log("SelectEndingStory" + endingStoryIndex);
		}
    }

	public void StartEndingStory()
	{
		Canvas.ForceUpdateCanvases();
	    story = new Story(inkJSONAssets[endingStoryIndex].text);
		if (OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}

    [SerializeField]
	private TextAsset[] inkJSONAssets;
	public Story story;

	[SerializeField]
	private GameObject canvas = null;

	// UI Prefabs
	[SerializeField]
	private TMP_Text TextPrefab = null; 
	[SerializeField]
	private TMP_Text textPrefabItalic = null;
	private TMP_Text storyText = null;
	[SerializeField]
	private Button buttonPrefab = null;
}
