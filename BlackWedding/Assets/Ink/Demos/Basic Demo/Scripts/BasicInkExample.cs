using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using TMPro;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;
	//public bool[] wasStoryTold;
	public int storyIndex = 0;
	public NPCInteraction NPC;

    void Awake () {
		// Remove the default message
		RemoveChildren();
		StartStory();
		NPC = GetComponentInParent<NPCInteraction>();
		NPC.quest.OnCompeletedQuest += LoadNewStory;
		NPC.OnOpenedUIPanel += StartStory;
	}

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory () 
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
	void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();
		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {
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
		//else {
		//	Button choice = CreateChoiceView("End of story.\nRestart?");
		//	choice.onClick.AddListener(delegate{
		//		StartStory();
		//	});
		//}
		else
		{
			NPC.Perform();
			NPC.CloseUIPanel();
		}

		if (story.currentTags.Contains("LoadNewScene"))
		{
			Button choice = CreateChoiceView("End of story.\nLoad new scene?");
			choice.onClick.AddListener(delegate
			{
				GameManager.Instance.LoadNextScene();
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
		if (story.currentTags.Count > 0)
		{
			storyText = Instantiate(textPrefabItalic) as TMP_Text;
			story.currentTags.Contains("Italic");
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

	void LoadNewStory()
    {
		
		if(inkJSONAssets.Length > 1 && storyIndex < inkJSONAssets.Length)
		{
			Debug.Log("was loaded new Story");
			storyIndex++;
			Canvas.ForceUpdateCanvases();
			StartStory();
		}
    }

	//[SerializeField]
	//private TextAsset currentInkJSONAsset = null;
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
