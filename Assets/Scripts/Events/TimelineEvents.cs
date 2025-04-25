using UnityEngine;
using System;
using UnityEngine.Playables;

public class TimelineEvents
{
    // Initialization of Timeline Events
    public event Action OnTimelineStarted; // Event for when the Timeline is Started
    public event Action OnTimelinePaused; // Event for when the Timeline is Paused
    public event Action OnTimelineFinished; // Event for when the Timeline is Finished
    public event Action<PlayableDirector> OnTimelineChanged; // Event for when the Timeline is Stopped

    // Functions for Invoking Events
    public void StartTimeline()
    {
        OnTimelineStarted?.Invoke();
    }
    public void PauseTimeline()
    {
        OnTimelinePaused?.Invoke();
    }
    public void FinishTimeline()
    {
        OnTimelineFinished?.Invoke();
    }
    public void ChangeTimeline(PlayableDirector playableDirector)
    {
        OnTimelineChanged?.Invoke(playableDirector);
    }
}
