using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class FinalNarrative : MonoBehaviour
{
    public static event Action<Story> OnCreateStory;

    public static string finalNumber = "finalOne";

    [SerializeField]
    private TextAsset inkJSONAsset = null;
    public Story story;

    [SerializeField]
    private Canvas canvas = null;

    // UI Prefabs
    [SerializeField]
    private Text textPrefab = null;
    private bool next;
    private bool keyDownEvent;
    private bool pause;
    public GameObject skipText;


    void Start()
    {
        GameObject.Find(finalNumber).GetComponent<Image>().enabled = true;
        next = false;
        //Remove the default message
        RemoveChildren();
        Invoke("StartStory", 2.3f);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.N) && keyDownEvent)
        {
            next = true;
            keyDownEvent = false;
            skipText.SetActive(false);

        }
    }

    // Creates a new Story object with the compiled story which we can then play!
    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        if (OnCreateStory != null) OnCreateStory(story);
        story.variablesState[finalNumber] = true;
        StartCoroutine(RefreshView());
    }

    // This is the main function called every time the story changes. It does a few things:
    // Destroys all the old content and choices.
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
    IEnumerator RefreshView()
    {
        next = false;
        // Remove all the UI on screen
        RemoveChildren();

        // Read all the content until we can't continue any more
        //do
        //{
        while (story.canContinue)
        {
            // Continue gets the next line of the story
            string text = story.Continue();
            // This removes any white space from the text.
            text = text.Trim();
            // Display the text on screen!
            CreateContentView(text);

            if (story.currentTags.Count > 0)
            {
                string tag = story.currentTags[0];
                if (tag == "pause")
                {
                    keyDownEvent = true;
                    pause = true;
                    skipText.SetActive(true);

                }

                if (pause)
                {
                    next = false;
                    pause = false;
                    yield return new WaitUntil(() => next);
                    RemoveChildren();
                }
            }
        }
        if (!story.canContinue)
        {
            GameObject.Find("TimeLineOut").GetComponent<PlayableDirector>().Play();
            RemoveChildren();   
        }
    }

    // Creates a textbox showing the the line of text
    void CreateContentView(string text)
    {
        Text storyText = Instantiate(textPrefab) as Text;
        storyText.text = text;
        storyText.transform.SetParent(canvas.transform, false);
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren()
    {
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            if (canvas.transform.GetChild(i).CompareTag("NarrativeObject"))
                GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }
    
}

