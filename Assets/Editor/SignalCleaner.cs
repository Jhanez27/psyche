using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class SignalCleaner : EditorWindow
{
    [MenuItem("Tools/Clean Missing Signals")]
    [System.Obsolete]
    public static void Cleanup()
    {
        // Clean missing signals in TimelineAssets
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

        // Clean missing signals in SignalReceiver components
        var signalReceivers = GameObject.FindObjectsOfType<SignalReceiver>();
        foreach (var receiver in signalReceivers)
        {
            bool hasInvalidEntries = false;

            for (int i = receiver.Count() - 1; i >= 0; i--)
            {
                var signal = receiver.GetSignalAssetAtIndex(i);
                if (signal == null)
                {
                    Debug.LogWarning($"SignalReceiver on GameObject '{receiver.gameObject.name}' has a missing signal at index {i}. Removing it.");
                    receiver.RemoveAtIndex(i);
                    hasInvalidEntries = true;
                }
            }

            if (hasInvalidEntries)
            {
                EditorUtility.SetDirty(receiver);
            }
        }

        Debug.Log("Signal cleanup complete.");
    }
}
