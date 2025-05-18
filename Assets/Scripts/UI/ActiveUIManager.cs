using UnityEngine;

public class ActiveUIManager : MonoBehaviour
{
    public static ActiveUIManager Instance { get; private set; } // Singleton instance of ActiveUIManager
    public ActiveUIType ActiveUIType { get; private set; } = ActiveUIType.None; // The current active UI type

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }
        else
            Instance = this; // Set the singleton instance
    }

    public bool CanOpenUI(ActiveUIType uiType)
    {
        return ActiveUIType == ActiveUIType.None; // Check if the UI can be opened
    }

    public void OpenUI(ActiveUIType uiType)
    {
        if (CanOpenUI(uiType))
        {
            ActiveUIType = uiType; // Set the active UI type
            // Additional logic to open the UI can be added here
        }
    }

    public void CloseUI(ActiveUIType uiType)
    {
        if (ActiveUIType == uiType)
        {
            ActiveUIType = ActiveUIType.None; // Reset the active UI type
            // Additional logic to close the UI can be added here
        }
    }
}
