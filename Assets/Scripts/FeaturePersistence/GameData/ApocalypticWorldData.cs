using UnityEngine;

[System.Serializable]
public class ApocalypticWorldData
{
    public Vector2 worldPosition;
    public string sceneName;
    public bool hasBeenLoadedBefore;
    public ApocalypticWorldData()
    {
        worldPosition = Vector2.zero;
        sceneName = string.Empty;
        hasBeenLoadedBefore = false;
    }

    public void InitialiszeApocalypticWorldPositionData(Vector2 worldPosition)
    {
        this.worldPosition = worldPosition;
    }

    public void InitializeApocalypticWorldSceneData(string name)
    {
        this.sceneName = name;
    }
}
