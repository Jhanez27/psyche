using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEditor.Timeline;

public class SignalCleaner : EditorWindow
{
    [MenuItem("Tools/Clean Missing Signals")]
    public static void Cleanup()
    {
        var guids = AssetDatabase.FindAssets("t:TimelineAsset");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(path);
            if (timeline == null) continue;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is SignalTrack signalTrack)
                {
                    foreach (var clip in signalTrack.GetClips())
                    {
                        var emitter = clip.asset as SignalEmitter;
                        if (emitter != null && emitter.asset == null)
                        {
                            Debug.LogWarning($"Missing signal in: {timeline.name} on clip '{clip.displayName}' — consider removing this clip.");
                        }
                    }
                }
            }
        }

        Debug.Log("Signal check complete.");
    }
}
