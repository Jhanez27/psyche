using UnityEngine;
using UnityEngine.Playables;

public class GamesEventManagerProxy : MonoBehaviour
{
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
}
