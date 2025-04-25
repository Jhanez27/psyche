using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    // Establishing a connection between Ink and Unity Functions
    public void Bind(Story story) // Bind external functions
    {
        story.BindExternalFunction("StartQuest", (string id) => StartQuest(id));
        story.BindExternalFunction("AdvanceQuest", (string id) => AdvanceQuest(id));
        story.BindExternalFunction("FinishQuest", (string id) => FinishQuest(id));
    }
    public void Unbind(Story story) // Unbind external functions
    {
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
    }

    // Functions to be called from Ink
    private void StartQuest(string id)
    {
        GamesEventManager.Instance.questEvents.StartQuest(id);
    }

    private void AdvanceQuest(string id)
    {
        GamesEventManager.Instance.questEvents.AdvanceQuest(id);
    }

    private void FinishQuest(string id)
    {
        GamesEventManager.Instance.questEvents.FinishQuest(id);
    }
}
