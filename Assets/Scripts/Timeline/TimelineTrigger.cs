using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] private string timelineID;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            if (hasTriggered && triggerOnce)
            {
                return;
            }
            GamesEventManager.Instance.timelineEvents.StartTimelineByID(timelineID);
            hasTriggered = true;
        }
    }
}
