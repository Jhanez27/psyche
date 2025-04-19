using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField]
    public string ID { get; private set; }

    [Header("General")]
    public string displayName;
    [TextArea]
    public string description;

    [Header("Requirements")]
    public QuestInfoSO[] questPrerequisite;

    [Header("Steps")]
    public GameObject[] questSteps;

    [Header("Rewards")] //Will be edited to Mementos and even items later
    public int experienceReward;
    public int goldReward;


    private void OnValidate()
    {
        #if UNITY_EDITOR
            ID = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
