using UnityEngine;

[System.Serializable]
public class JaneWorldData
{
    public Vector2 worldPosition;
    public string sceneName;
    public bool hasBeenLoadedBefore;

    public JaneWorldData()
    {
        worldPosition = Vector2.zero;
        sceneName = string.Empty;
        hasBeenLoadedBefore = false;
    }

    public void InitialiszeJaneWorldPositionData(Vector2 worldPosition)
    {
        this.worldPosition = worldPosition;
    }

    public void InitializeJaneWorldSceneData(string name)
    {
        this.sceneName = name;
    }
}
