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

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Instance Already Exists");
            return;
        }

        Debug.Log("Instantiating GamesEventManager");
        Instance = this;

        // Initialization of All Listed Game Events
        this.inputEvents = new InputEvents();
        this.playerEvents = new PlayerEvents();
        this.inventoryUIEvents = new InventoryUIEvents();
        this.inventoryModelEvents = new InventoryModelEvents();
        this.questEvents = new QuestEvents();
    }
}
