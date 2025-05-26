using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isOnApocalypticWorld = true;
    public ApocalypticWorldData apocalypticWorldData = new ApocalypticWorldData();
    public JaneWorldData janeWorldData = new JaneWorldData();
    public TimelineData timelineData = new TimelineData();
    public List<QuestDataEntry> questDataList = new List<QuestDataEntry>();
    public List<InventoryData> inventoryDataList = new List<InventoryData>();
    public List<DialogueData> dialogueVariableData = new List<DialogueData>();
    public List<SoftGateData> softGateDataList = new List<SoftGateData>();

    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("=== GameData ===");

        // Timeline Data
        sb.AppendLine("--- TimelineData ---");
        sb.AppendLine(timelineData?.ToString() ?? "No TimelineData");

        // Quest Data
        sb.AppendLine("--- QuestDataList ---");
        if (questDataList == null || questDataList.Count == 0)
        {
            sb.AppendLine("No quests saved.");
        }
        else
        {
            foreach (var entry in questDataList)
            {
                sb.AppendLine($"Quest ID: {entry.questID}");
                sb.AppendLine(entry.questData?.ToString() ?? "  Null QuestData");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}

[System.Serializable]
public class QuestDataEntry
{
    public string questID;
    public QuestData questData;

    public QuestDataEntry(string questID, QuestData questData)
    {
        this.questID = questID;
        this.questData = questData;
    }
}
