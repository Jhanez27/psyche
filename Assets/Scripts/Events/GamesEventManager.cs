using UnityEngine;

public class GamesEventManager : MonoBehaviour
{
    // Singleton Pattern for GamesEventManager
    public static GamesEventManager Instance { get; private set; }

    // All Game Events 
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        // Initialization of All Listed Game Events
        this.inputEvents = new InputEvents();
        this.playerEvents = new PlayerEvents();
    }
}
