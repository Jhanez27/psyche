using System;
using UnityEngine;

public class QuestUIEvents 
{
    // Initialization of QuestUI Events
    public event Action<string> OnLogItemClicked; // Event for when the QuestLogItem is Clicked

    //Functions for Invoking Events
    public void LogItemClicked(string id)
    {
        OnLogItemClicked?.Invoke(id);
    }
}
