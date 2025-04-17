using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene Trigger Ink JSON")]
    [SerializeField] private TextAsset cutsceneJSON;
    
    public void StartCutscene()
    {
        if(DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(cutsceneJSON);
        }
        else
        {
            Debug.LogError("DialogueManager instance is null. Make sure it is initialized before calling StartDialogue.");
        }
    }
}
