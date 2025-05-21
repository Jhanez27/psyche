using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimelineData
{
    public List<TimelineState> timelineStates = new List<TimelineState>();

    public bool HasTimelinePlayed(string id)
    {
        var entry = timelineStates.Find(t => t.timelineID == id);
        return entry != null && entry.hasPlayed;
    }

    public void SetTimelinePlayed(string id)
    {
        var entry = timelineStates.Find(t => t.timelineID == id);
        if (entry != null)
        {
            entry.hasPlayed = true;
        }
        else
        {
            timelineStates
                .Add(new TimelineState
                {
                    timelineID = id,
                    hasPlayed = true
                });
        }
    }
}

[System.Serializable]
public class TimelineState
{
    public string timelineID;
    public bool hasPlayed;
}
