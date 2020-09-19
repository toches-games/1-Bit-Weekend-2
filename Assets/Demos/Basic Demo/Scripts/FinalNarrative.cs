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

    public static string finalNumber = "finalTwo";

    [SerializeField]
    private TextAsset inkJSONAsset = null;
    public Story story;

    [SerializeField]
    private Canvas canvas = null;

    // UI Prefabs
    [SerializeField]
    private Text playerTitlePrefab = null;
    [SerializeField]
    private Text nPCTitlePrefab = null;
    [SerializeField]
    private Text textPrefab = null;
    private bool next;
    private bool keyDownEvent;
    private bool pause;
    private bool skip;
    public GameObject skipText;
    private bool finishCoroutine = false;
    private const string NAME_PLAYER = "FORASTERO";
    private const string NAME_NPC = "VIDENTE";

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

        if (Input.GetMouseButtonDown(0) && keyDownEvent)
        {
            keyDownEvent = false;
            skip = true;

            if (finishCoroutine)
            {
                next = true;
                skipText.SetActive(false);
            }

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
        // Remove all the UI on screen
        RemoveChildren();

        // Read all the content until we can't continue any more
        //do
        //{
        while (story.canContinue)
        {
            // Continue gets the next line of the story
            string text = story.Continue();
            string tag = "";
            // This removes any white space from the text.
            text = text.Trim();

            pause = false;
            next = false;
            keyDownEvent = false;

            if (story.currentTags.Count > 0)
            {
                tag = story.currentTags[0];
                if (tag == "pause")
                {
                    keyDownEvent = true;
                    pause = true;
                    skipText.SetActive(true);

                }               
                else
                {
                    skipText.SetActive(false);
                }
            }

            yield return StartCoroutine(CreateContentView(text));

            if (pause)
            {
                yield return new WaitUntil(() => next);

                RemoveChildren();
            }

        }
        if (!story.canContinue)
        {
            GameObject.Find("TimeLineOut").GetComponent<PlayableDirector>().Play();
            RemoveChildren();   
        }
    }

    // Creates a textbox showing the the line of text
    IEnumerator CreateContentView(string text)
    {
        Text storyText = null;
        int lengthText = text.Length, i = 0;
        //Text storyText = Instantiate(textPrefab) as Text;
        keyDownEvent = true;
        skip = false;
        finishCoroutine = false;

        if (text == NAME_PLAYER || text == NAME_NPC)
        {
            if (text == NAME_NPC)
            {
                storyText = Instantiate(playerTitlePrefab) as Text;
            }
            else
            {
                storyText = Instantiate(nPCTitlePrefab) as Text;
            }
            storyText.transform.SetParent(canvas.transform, false);
            storyText.text = text;
            yield return new WaitForSeconds(0.35f);
        }else{
            storyText = Instantiate(textPrefab) as Text;
            storyText.transform.SetParent(canvas.transform, false);

            for (i = 0; i < lengthText; i++)
            {
                if (skip)
                {
                    storyText.text = storyText.text + text.Substring(i);
                    keyDownEvent = true;
                    break;
                }
                char c = text[i];
                storyText.text = storyText.text + c;
                yield return new WaitForSeconds(0.09f);
            }
        }
        if (i == lengthText)
        {
            yield return new WaitForSeconds(0.9f);
            next = true;
        }
        finishCoroutine = true;
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

