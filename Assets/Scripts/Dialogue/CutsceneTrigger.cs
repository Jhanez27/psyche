using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene Trigger Ink JSON")]
    [SerializeField] private TextAsset cutsceneJSON;
    
    void Awake()
    {
        StartCoroutine(DelayedStart()        );
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.5f);
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
