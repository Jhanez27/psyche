using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Cutscene Trigger")]
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
            DialogueManager.Instance.EnterDialogueMode(cutsceneJSON);
        }
        else
        {
            Debug.LogError("DialogueManager instance is null. Make sure it is initialized before calling EnterDialogueMode.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
