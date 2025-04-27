using UnityEngine;
using UnityEngine.Playables;

public class TimelineHandler : MonoBehaviour
{
    [Header("Timeline Elements")]
    [SerializeField] private PlayableDirector timeline; //Panel for displaying the timeline

    public bool TimelineIsActive => (timeline != null) && (timeline.time < timeline.duration && timeline.state.Equals(PlayState.Paused));

    public void OnEnable()
    {
        DialogueManager.Instance.OnDialogueEnd += PlayTimeline; //Subscribes to the dialogue end event    
    }

    public void OnDisable()
    {
        DialogueManager.Instance.OnDialogueEnd -= PlayTimeline; //Unsubscribes from the dialogue end event
    }

    public void PlayTimeline()
    {
        if (timeline != null && !timeline.state.Equals(PlayState.Playing)) //Checks if the timeline is not already playing
        {
            if(timeline.time == 0) //Checks if the timeline is at the beginning
            {
                timeline.Play(); //Plays the timeline from the beginning
            }
            else
            {
                timeline.Resume(); //Resumes the timeline from the current time
            }
        }
    }

    public void StopTimeline()
    {
        if (timeline != null && timeline.state.Equals(PlayState.Playing)) //Checks if the timeline is playing
        {
            timeline.Stop(); //Stops the timeline
            DialogueManager.Instance.TimelineIsActive = false; //Sets the timeline state to not playing
        }
    }

    public void PauseTimeline()
    {
        if (timeline != null && timeline.state.Equals(PlayState.Playing)) //Checks if the timeline is playing
        {
            timeline.Pause(); //Pauses the timeline
            DialogueManager.Instance.TimelineIsActive = false; //Sets the timeline state to not playing
        }
    }
}
