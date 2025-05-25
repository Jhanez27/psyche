using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class SoftGatwithKnotBasis : MonoBehaviour
{
    [SerializeField] private string knotBasis = string.Empty;

    public void OnEnable()
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
}
