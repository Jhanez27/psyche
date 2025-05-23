using System;
using UnityEngine;

public class DialogueBasedQuestIconChange : MonoBehaviour
{
    [Header("Quest Icon")]
    [SerializeField]
    private QuestIcon questIcon;

    [Header("KnotName Basis")]
    [SerializeField]
    private string knotName;


    private void Start()
    {
        
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered += ChangeIconOnDialogueEntered;
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered += ChangeIconOnDialogueEntered;
    }

    private void ChangeIconOnDialogueEntered(string knotName, DialogueSource source)
    {
    }
}
