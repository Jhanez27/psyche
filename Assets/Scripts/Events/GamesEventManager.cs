using UnityEngine;

public class GamesEventManager : MonoBehaviour
{
    // Singleton Pattern for GamesEventManager
    public static GamesEventManager Instance { get; private set; }

    // All Game Events 
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public InventoryUIEvents inventoryUIEvents;
    public InventoryModelEvents inventoryModelEvents;
    public QuestEvents questEvents;
    public QuestUIEvents questUIEvents;
    public DialogueEvents dialogueEvents;
    public TimelineEvents timelineEvents;

    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;

        // Initialization of All Listed Game Events
        this.inputEvents = new InputEvents();
        this.playerEvents = new PlayerEvents();
        this.inventoryUIEvents = new InventoryUIEvents();
        this.inventoryModelEvents = new InventoryModelEvents();
        this.questEvents = new QuestEvents();
        this.questUIEvents = new QuestUIEvents();
        this.dialogueEvents = new DialogueEvents(); 
        this.timelineEvents = new TimelineEvents();
    }
}
