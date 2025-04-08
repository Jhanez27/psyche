using System;
using Ink.Runtime;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    /// Singleton instance of StoryManager
    public static StoryManager Instance { get; private set; }

    //StoryManager events
    public event Action<Story> OnStoryStart;
    public event Action<Story> OnLineUpdate;
    public event Action<Story> OnChoiceUpdate;
    public event Action<Story> OnStoryEnd;

    // StoryManager properties
    private Story currentStory;
    private bool isStoryActive = false;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of StoryManager exists
        if (Instance == null) { Instance = this;}
        else { Destroy(gameObject);} 
    }

    public void StartStory(TextAsset inkJSON)
    {
        // Check if a story is already active
        if (isStoryActive) return; 

        //Load the story from the JSON file
        currentStory = new Story(inkJSON.text);
        isStoryActive = true;
        OnStoryStart?.Invoke(currentStory);
        ContinueStory();
    }

    public void ContinueStory()
    {
        // Check if the story is active and has more content to display
        if (isStoryActive && currentStory.canContinue)
        {
            string line = currentStory.Continue(); 
            OnLineUpdate?.Invoke(currentStory); 

            //Log the line to the console
            Debug.Log(line); 

            if(currentStory.currentChoices.Count > 0)
            {
                // If there are choices available, update the choices
                OnChoiceUpdate?.Invoke(currentStory); 
            }
            else
            {
                // If there are no choices, continue the story
                ContinueStory(); 
            }
        }
        else if (currentStory.canContinue == false)
        {
            EndStory(); // End the story if there are no more lines to display
        }
    }

    public void EndStory()
    {
        // Check if the story is active before ending it
        if (!isStoryActive) return; 

        isStoryActive = false; 
        OnStoryEnd?.Invoke(currentStory); // Invoke the end event
        currentStory = null; // Reset the current story
    }
}
