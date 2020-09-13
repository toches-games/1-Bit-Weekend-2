using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
    public static event Action<Story> OnCreateStory;

    [SerializeField]
    private TextAsset inkJSONAsset = null;
    public Story story;

    [SerializeField]
    private Canvas canvas = null;

    // UI Prefabs
    [SerializeField]
    private Text textPrefab = null;
    [SerializeField]
    private Button buttonPrefab = null;
    [SerializeField]
    private InputField inputPrefab = null;
    private bool next;
    [SerializeField]
    private List<string> variablesOfDecision = new List<string>();
    private bool keyDownEvent;

    void Awake () {
        next = false;
		//Remove the default message
		RemoveChildren();
		StartStory();
	}

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.N) && keyDownEvent)
        {
            next = true;
            keyDownEvent = false;
        }
    }

    // Creates a new Story object with the compiled story which we can then play!
    void StartStory () {
		story = new Story (inkJSONAsset.text);
        if(OnCreateStory != null) OnCreateStory(story);
		StartCoroutine(RefreshView());
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	IEnumerator RefreshView () {
        next = false;
        // Remove all the UI on screen
        RemoveChildren ();

        // Read all the content until we can't continue any more
        //do
        //{
        while(story.canContinue)
        {
            // Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
            // Display the text on screen!
            CreateContentView(text);

            if(story.currentTags.Count > 0)
            {
                string tag = story.currentTags[0];
                if(tag == "pause")
                {
                    keyDownEvent = true;
                }
                else if(tag == "fecha")
                {
                    InputField input = CreateInputField(1);
                }else if(tag == "ciudad")
                {
                    InputField input = CreateInputField(2);
                }
                yield return new WaitUntil(() => next);
                next = false;
                RemoveChildren();
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
		else if(!story.canContinue) {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		StartCoroutine(RefreshView());
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

    InputField CreateInputField(int type)
    {
        // Creates the button from a prefab
        InputField input = Instantiate(inputPrefab) as InputField;
        input.transform.SetParent(canvas.transform, false);
        if(type == 1)
        {
            input.contentType = InputField.ContentType.IntegerNumber;
            input.onEndEdit.AddListener(delegate
            {
                next = true;
                ProcessingVariablesOfDecision(input.text, 1);
            });
        }
        else
        {
            input.contentType = InputField.ContentType.Name;
            input.characterLimit = 0;
            input.onEndEdit.AddListener(delegate
            {
                next = true;
                ProcessingVariablesOfDecision(input.text, 2);
            });

        }
        input.ActivateInputField();
        input.Select();
        return input;
    }

    public void ProcessingVariablesOfDecision(string text, int type)
    {
        story.variablesState["dataError"] = false;
        if (type == 1)
        {
            if (!Int32.TryParse(text, out int j) || j < 1960 || j > 2020)
            {
                story.variablesState["dataError"] = true;
                return;
            }
        }
        else
        {
            if(text.Length < 3 || text.Length > 30)
            {
                story.variablesState["dataError"] = true;
                return;
            }
        }

        variablesOfDecision.Add(text);
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

}
