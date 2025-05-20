using Ink.Runtime;
using NUnit.Framework.Constraints;
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
    private bool hasShowSpriteVariableBasis = true;
    [SerializeField]
    private string inkVariableNameBasis = string.Empty;


    private void Awake()
    {
        questIconRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (!hasShowSpriteVariableBasis)
        {
            ClearQuestIcon();
        }
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.dialogueEvents.OnInkVariableChanged += ClearQuestIconByVariableBasis;
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnInkVariableChanged -= ClearQuestIconByVariableBasis;
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
    public void ClearQuestIconByVariableBasis(string variableBasis, Ink.Runtime.Object state)
    {
        if (variableBasis != inkVariableNameBasis)
        {
            return;
        }

        if (state is BoolValue boolValue)
        {
            if (boolValue)
            {
                ClearQuestIcon();
            }
        }
    }
}
