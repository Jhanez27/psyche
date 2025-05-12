using System;
using UnityEngine;

public class QuestUIEvents 
{
    // Initialization of QuestUI Events
    public event Action<QuestLogItem> OnLogItemClicked; // Event for when the QuestLogItem is Clicked

    public event Action<string> OnDescriptionRequested;

    //Functions for Invoking Events
    public void LogItemClicked(QuestLogItem logItem)
    {
        OnLogItemClicked?.Invoke(logItem);
    }

    public void DescriptionRequested(string id)
    {
        OnDescriptionRequested?.Invoke(id);
    }
}
