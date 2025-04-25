using System;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField]
    private QuestInfoSO questInfoForPoint; // Quest to complete

    [Header("Quest Configurations")]
    [SerializeField]
    private bool startPoint = true; // The interaction will be the starting point
    [SerializeField]
    private bool finishPoint = true; // The interaction will be the end point

    [Header("Quest Dialogue Configuration")]
    [SerializeField]
    private string dialogueKnotName; // The name of the knot to be used in the dialogue system

    private bool playerIsNear = false;
    private bool questInteractEnabled = true; // Whether the quest log toggle is enabled
    private string questID;
    private QuestState currentQuestState;
    

    private void Awake()
    {
        questID = questInfoForPoint.ID;
    }

    private void OnEnable()
    {
        GamesEventManager.Instance.questEvents.OnChangeQuestState += QuestStateChange;
        GamesEventManager.Instance.inputEvents.OnInteractPressed += InteractPressed;
        GamesEventManager.Instance.questEvents.OnInteractEnabled += EnableQuestInteract;
        GamesEventManager.Instance.questEvents.OnInteractDisabled += DisableQuestInteract;
    }

    private void OnDisable()
    {
        GamesEventManager.Instance.questEvents.OnChangeQuestState -= QuestStateChange;
        GamesEventManager.Instance.inputEvents.OnInteractPressed -= InteractPressed;
        GamesEventManager.Instance.questEvents.OnInteractEnabled -= EnableQuestInteract;
        GamesEventManager.Instance.questEvents.OnInteractDisabled -= DisableQuestInteract;
    }

    private void QuestStateChange(Quest quest)
    {
        if(quest.questInfo.ID.Equals(questID))
        {
            currentQuestState = quest.state;
        }
    }

    private void InteractPressed(InputEventContext context)
    {
        if (playerIsNear && context.Equals(InputEventContext.DEFAULT) && questInteractEnabled)
        {
            if (!dialogueKnotName.Equals(string.Empty))
            {
                ActiveUIManager.Instance.OpenUI(ActiveUIType.Dialogue); // Open the dialogue UI
                GamesEventManager.Instance.dialogueEvents.EnterDialogue(dialogueKnotName, DialogueSource.GAMEPLAY);
            }
            else
            {
                if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
                {
                    GamesEventManager.Instance.questEvents.StartQuest(questID);
                }
                else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
                {
                    GamesEventManager.Instance.questEvents.FinishQuest(questID);
                }
            }

            Debug.Log("Interacted with " + tag);
            GamesEventManager.Instance.questEvents.InteractInCollision(this.gameObject.tag); // Pass the tag for Collision
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    private void EnableQuestInteract()
    {
        questInteractEnabled = true;
    }
    private void DisableQuestInteract()
    {
        questInteractEnabled = false;
    }
}
