using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class SoftGatwithKnotBasis : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string knotBasis = string.Empty;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }    public void OnEnable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered += CheckKnotBasis;
    }
    public void OnDisable()
    {
        GamesEventManager.Instance.dialogueEvents.OnDialogueEntered -= CheckKnotBasis;
    }

    private void CheckKnotBasis(string knotBasisName, DialogueSource source)
    {
        if (knotBasisName.Equals(knotBasis))
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }
    }

    public void LoadData(GameData data)
    {
        bool found = false;
        foreach (SoftGateData softGateData in data.softGateDataList)
        {
            if (softGateData.softGateName.Equals(gameObject.name))
            {
                boxCollider.isTrigger = softGateData.isActive;
                found = true;
                break;
            }
        }
        if (!found)
        {
            // Add new SoftGateData if not found
            SoftGateData newData = new SoftGateData
            {
                softGateName = gameObject.name,
                isActive = boxCollider.isTrigger
            };
            data.softGateDataList.Add(newData);
        }
    }

    public void SaveData(ref GameData data)
    {
        bool found = false;
        foreach (SoftGateData softGateData in data.softGateDataList)
        {
            if (softGateData.softGateName == gameObject.name)
            {
                softGateData.isActive = boxCollider.isTrigger;
                found = true;
                break;
            }
        }
        if (!found)
        {
            // Add new SoftGateData if not found
            SoftGateData newData = new SoftGateData
            {
                softGateName = gameObject.name,
                isActive = boxCollider.isTrigger
            };
            data.softGateDataList.Add(newData);
        }
    }
}
