using UnityEngine;
using UnityEngine.Playables;

public class GamesEventManagerProxy : MonoBehaviour
{
    [Header("Change Dialogue Knot Settings")]
    public string npcTag;
    public string dialogueName;

    public void TriggerDialogueEntered(string knotName)
    {
        GamesEventManager.Instance.dialogueEvents.EnterDialogue(knotName, DialogueSource.TIMELINE);
    }
    public void TriggerStartTimeline()
    {
        GamesEventManager.Instance.timelineEvents.StartTimeline();
    }
    public void TriggerPauseTimeline()
    {
        GamesEventManager.Instance.timelineEvents.PauseTimeline();
    }
    public void TriggerFinishTimeline()
    {
        GamesEventManager.Instance.timelineEvents.FinishTimeline();
    }
    public void TriggerChangeDialogueKnotName()
    {
        GamesEventManager.Instance.questEvents.ChangeDialogueName(npcTag, dialogueName);
    }
}
