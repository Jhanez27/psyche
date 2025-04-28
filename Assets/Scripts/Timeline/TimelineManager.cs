using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [Header("Timeline Elements")]
    [SerializeField] private PlayableDirector timeline; //Panel for displaying the timeline

    public bool TimelineIsActive => (timeline != null) && (timeline.time < timeline.duration && timeline.state.Equals(PlayState.Paused));

    private void OnEnable()
    {
        GamesEventManager.Instance.timelineEvents.OnTimelineStarted += PlayTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelinePaused += PauseTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelineFinished += FinishTimeline; //Subscribes to the timeline finished event
        GamesEventManager.Instance.timelineEvents.OnTimelineChanged += ChangeTimeline;
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.timelineEvents.OnTimelineStarted -= PlayTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelinePaused -= PauseTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelineFinished -= FinishTimeline; //Unsubscribes from the timeline finished event
        GamesEventManager.Instance.timelineEvents.OnTimelineChanged -= ChangeTimeline;
    }

    // Functions for Timeline Progression
    private void PlayTimeline()
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

        GamesEventManager.Instance.playerEvents.MovementDisabled(); //Disables player movement
        GamesEventManager.Instance.dialogueEvents.DisableNext(); //Disables the next button.
        GamesEventManager.Instance.inventoryUIEvents.DisableInventory(); //Disables the inventory UI
        GamesEventManager.Instance.questEvents.DisableInteract(); //Disables the quest log UI
        Debug.Log("Movement, Next, Inventory, and Interact Disabled");
    }
    private void PauseTimeline()
    {
        if (timeline != null && timeline.state.Equals(PlayState.Playing)) //Checks if the timeline is playing
        {
            timeline.Pause(); //Pauses the timeline
            GamesEventManager.Instance.dialogueEvents.EnableNext(); //Enables the next button
            GamesEventManager.Instance.questEvents.EnableInteract(); //Enables the quest log UI
        }
    }
    private void FinishTimeline()
    {
        GamesEventManager.Instance.playerEvents.MovementEnabled(); //Enables player movement
        GamesEventManager.Instance.dialogueEvents.EnableNext(); //Enables the next button
        GamesEventManager.Instance.inventoryUIEvents.EnableInventory(); //Enables the inventory UI
        GamesEventManager.Instance.questEvents.EnableInteract(); //Enables the quest log UI

        Debug.Log("Movement, Next, Inventory, and Interact Enabled");
        Debug.Log("Active UI Type: " + ActiveUIManager.Instance.ActiveUIType); //Logs the active UI type
    }

    // Function for Changing Timeline
    private void ChangeTimeline(PlayableDirector newTimeline)
    {
        timeline = newTimeline; //Changes the timeline to the new timeline
    }
}
