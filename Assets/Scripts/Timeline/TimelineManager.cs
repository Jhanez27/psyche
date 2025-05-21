using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour, IDataPersistence
{
    [System.Serializable]
    public class TimelineEntry
    {
        public string timelineID;
        public TimelineAsset timelineAsset;
        public bool playOnSceneEnter;
    }

    [Header("Timeline Elements")]
    [SerializeField] 
    private PlayableDirector timeline; //Panel for displaying the timeline
    [SerializeField]
    private List<TimelineEntry> timelines = new();

    private HashSet<string> playedTimelines = new HashSet<string>();

    public bool TimelineIsActive => (timeline != null) && (timeline.time < timeline.duration && timeline.state.Equals(PlayState.Paused));

    private void Awake()
    {
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.timelineEvents.OnTimelineStartedByID += TryPlayTimelineByID;
        GamesEventManager.Instance.timelineEvents.OnTimelineStartedOnEntrance += PlayOnSceneEnter;
        GamesEventManager.Instance.timelineEvents.OnTimelineResumed += ResumeTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelinePaused += PauseTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelineFinished += FinishTimeline; //Subscribes to the timeline finished event
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.timelineEvents.OnTimelineStartedByID -= TryPlayTimelineByID;
        GamesEventManager.Instance.timelineEvents.OnTimelineStartedOnEntrance -= PlayOnSceneEnter;
        GamesEventManager.Instance.timelineEvents.OnTimelineResumed -= ResumeTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelinePaused -= PauseTimeline;
        GamesEventManager.Instance.timelineEvents.OnTimelineFinished -= FinishTimeline; //Unsubscribes from the timeline finished event
    }

    // Functions for Timeline Progression
    public void TryPlayTimelineByID(string id)
    {
        TimelineEntry entry = timelines.Find(t => t.timelineID == id);
        if (entry == null)
        {
            Debug.LogWarning($"Timeline '{id}' is not found.");
            return;
        }

        if(playedTimelines.Contains(id))
        {
            Debug.Log($"Timeline '{id}' has already been played.");
            return;
        }

        if(timeline.playableAsset != entry.timelineAsset)
            timeline.playableAsset = entry.timelineAsset;

        DisableFeaturesDuringTimelinePlay();
        timeline.Play();

    }
    public void PlayOnSceneEnter()
    {
        foreach (TimelineEntry entry in timelines)
        {
            if(entry.playOnSceneEnter)
            {
                TryPlayTimelineByID((string)entry.timelineID);
                break;
            }
        }
    }
    private void ResumeTimeline()
    {
        timeline.Resume();
        DisableFeaturesDuringTimelinePlay();
    }
    private void PauseTimeline()
    {
        if (timeline != null && timeline.state.Equals(PlayState.Playing)) //Checks if the timeline is playing
        {
            timeline.Pause(); //Pauses the timeline
            EnableFeaturesDuringTimelinePause();
            timeline.Evaluate();
        }
    }
    private void FinishTimeline()
    {
        EnableFeaturesAfterTimelineFinish();

        var currentTimeline = timelines.Find(t => t.timelineAsset == timeline.playableAsset);
        playedTimelines.Add(currentTimeline.timelineID);
    }

    // Function for DIsabling and Enabling Features during Timeline
    private void DisableFeaturesDuringTimelinePlay()
    {
        GamesEventManager.Instance.playerEvents.MovementDisabled(); //Disables player movement
        GamesEventManager.Instance.dialogueEvents.DisableNext(); //Disables the next button.
        GamesEventManager.Instance.inventoryUIEvents.DisableInventory(); //Disables the inventory UI
        GamesEventManager.Instance.questEvents.DisableInteract(); //Disables the quest log UI
    }
    private void EnableFeaturesDuringTimelinePause()
    {
        GamesEventManager.Instance.dialogueEvents.EnableNext(); //Enables the next button
        GamesEventManager.Instance.questEvents.EnableInteract(); //Enables the quest log UI
    }
    private void EnableFeaturesAfterTimelineFinish()
    {
        GamesEventManager.Instance.playerEvents.MovementEnabled(); //Enables player movement
        GamesEventManager.Instance.dialogueEvents.EnableNext(); //Enables the next button
        GamesEventManager.Instance.inventoryUIEvents.EnableInventory(); //Enables the inventory UI
        GamesEventManager.Instance.questEvents.EnableInteract(); //Enables the quest log UI
    }

    // Function for Data Persistence
    public void LoadData(GameData gameData)
    {
        playedTimelines.Clear();
        foreach (var entry in gameData.timelineData.timelineStates)
        {
            if (gameData.timelineData.HasTimelinePlayed(entry.timelineID))
                playedTimelines.Add(entry.timelineID);
        }
    }

    public void SaveData(ref GameData gameData)
    {
        foreach (string id in playedTimelines)
        {
            gameData.timelineData.SetTimelinePlayed(id);
        }
    }
}
