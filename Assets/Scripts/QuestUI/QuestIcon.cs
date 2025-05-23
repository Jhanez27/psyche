using Ink.Runtime;
using NUnit.Framework.Constraints;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    private SpriteRenderer questIconRenderer;
    [SerializeField]
    private Sprite happyEmote;
    [SerializeField]
    private Sprite sadEmote;
    [SerializeField]
    private Sprite dialogueEmote;
    [SerializeField]
    private Sprite angryEmote;
    [SerializeField]
    private Sprite shockEmote;
    [SerializeField]
    private Sprite questionEmote;
    [SerializeField]
    private Sprite sideEyeEmote; 
    [SerializeField]
    private Sprite exclamationEmote;

    [Header("Icon Configuration")]
    [SerializeField]
    private string inkVariableNameBasis = string.Empty;
    [SerializeField]
    private string knotNameBasis = string.Empty;


    private void Awake()
    {
        questIconRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (string.IsNullOrEmpty(inkVariableNameBasis))
        {
            ClearQuestIcon();
        }
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.dialogueEvents.OnInkVariableChanged += ShowQuestIconByVariableBasis;
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered += ClearIconOnDialogueEntered;
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnInkVariableChanged -= ShowQuestIconByVariableBasis;
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered -= ClearIconOnDialogueEntered;
    }

    //Quest Icon Display
    public void ClearQuestIcon()
    {
        questIconRenderer.sprite = null;
    }
    public void ShowHappyIcon()
    {
        questIconRenderer.sprite = happyEmote;
    }
    public void ShowSadIcon()
    {
        questIconRenderer.sprite = sadEmote;
    }
    public void ShowDialogueIcon()
    {
        questIconRenderer.sprite = dialogueEmote;
    }
    public void ShowAngryIcon()
    {
        questIconRenderer.sprite = angryEmote;
    }
    public void ShowShockIcon()
    {
        questIconRenderer.sprite = shockEmote;
    }
    public void ShowQuestionIcon()
    {
        questIconRenderer.sprite = questionEmote;
    }    
    public void ShowRBFIcon()
    {
        questIconRenderer.sprite = sideEyeEmote;
    }
    public void ShowExclamationIcon()
    {
        questIconRenderer.sprite = exclamationEmote;
    }
    public void ShowQuestIconByVariableBasis(string variableBasis, Ink.Runtime.Object state)
    {
        Debug.Log($"{variableBasis} and {inkVariableNameBasis}");
        if (variableBasis.Equals(this.inkVariableNameBasis))
        {
            Debug.Log($"{variableBasis} and {inkVariableNameBasis}");
            if (state is BoolValue boolValue)
            {
                Debug.Log(boolValue.value); // This is the actual C# bool
                if (boolValue.value)
                {
                    ShowDialogueIcon();
                }
            }
        }
    }
    private void ClearIconOnDialogueEntered(string knotName, DialogueSource source)
    {
        Debug.Log(knotName);
        if(knotName.Equals(knotNameBasis))
        {
            ClearQuestIcon();
        }
    }

    public void SetIconActive()
    {
        this.questIconRenderer.enabled = true;
    }    
    public void SetIconInactive()
    {
        this.questIconRenderer.enabled = false;
    }
}
