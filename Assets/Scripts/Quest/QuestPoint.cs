using System;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField]
    private QuestInfoSO questInfoForPoint;

    [Header("Quest Configurations")]
    [SerializeField]
    private bool startPoint = true;
    [SerializeField]
    private bool finishPoint = true;
    
    private QuestInputHandler inputHandler;

    private bool playerIsNear = false;
    private string questID;
    private QuestState currentQuestState;

    private void Awake()
    {
        inputHandler = GetComponent<QuestInputHandler>();
        questID = questInfoForPoint.ID;
    }

    private void OnEnable()
    {
        QuestManager.Instance.OnChangeQuestState += QuestStateChange;
    }

    private void OnDisable()
    {
        QuestManager.Instance.OnChangeQuestState -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if(quest.questInfo.ID.Equals(questID))
        {
            currentQuestState = quest.state;

        }
    }

    private void Update()
    {
        if(inputHandler.inputSystem.Player.Interact.triggered)
        {
            SubmitPressed();
        }
    }

    private void SubmitPressed()
    {
        if (playerIsNear)
        {
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                QuestManager.Instance.StartQuest(questID);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                QuestManager.Instance.FinishQuest(questID);
            }
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
}
