using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueBasedExit : MonoBehaviour
{
    [Header("Dialogue Based Exit Configurations")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField]
    private string waitingOnKnotNameEnter = string.Empty;
    private bool knotNameEnteredDialogue = false;

    private float waitToLoadTime = 1f;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered += ChangeEnterStatus;
        GamesEventManager.Instance.dialogueEvents.OnDialogueFinished += ProceedToExit;
    }
    private void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered -= ChangeEnterStatus;
        GamesEventManager.Instance.dialogueEvents.OnDialogueFinished -= ProceedToExit;
    }

    private void ChangeEnterStatus(string knotName, DialogueSource source)
    {
        if (knotName == waitingOnKnotNameEnter)
        {
            knotNameEnteredDialogue = true;
        }
    }
    private void ProceedToExit()
    {
        if (knotNameEnteredDialogue)
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            UIFade.Instance.FadeToWhite();
            StartCoroutine(LoadSceneRoutine());
        }
    }
    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene(sceneToLoad);
    }




}
