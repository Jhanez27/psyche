using UnityEngine;
using System;
using UnityEngine.Playables;

public class TimelineEvents
{
    // Initialization of Timeline Events
    public event Action<string> OnTimelineStartedByID;
    public event Action OnTimelineStartedOnEntrance;
    public event Action OnTimelineStarted; // Event for when the Timeline is Started
    public event Action OnTimelinePaused; // Event for when the Timeline is Paused
    public event Action OnTimelineResumed; // Event for when Timeline is Resumed
    public event Action OnTimelineFinished; // Event for when the Timeline is Finished

    // Functions for Invoking Events
    public void StartTimelineByID(string id)
    {
        OnTimelineStartedByID?.Invoke(id);
    }
    public void StartTimelineOnEntrance()
    {
        OnTimelineStartedOnEntrance?.Invoke();
    }
    public void StartTimeline()
    {
        OnTimelineStarted?.Invoke();
    }
    public void PauseTimeline()
    {
        OnTimelinePaused?.Invoke();
    }
    public void ResumeTimeline()
    {
        OnTimelineResumed?.Invoke();
    }
    public void FinishTimeline()
    {
        OnTimelineFinished?.Invoke();
    }
}
