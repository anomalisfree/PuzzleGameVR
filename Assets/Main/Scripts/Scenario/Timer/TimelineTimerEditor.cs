#if UNITY_EDITOR
using System;
using Main.Scripts.ApplicationCore.RealtimeModels;
using UnityEditor;
using UnityEngine;

namespace Main.Scripts.Scenario.Timer
{
    [CustomEditor(typeof(TimelineTimerData))]
    public class TimelineTimerEditor : Editor
    {
        private float _timeToSet = 0f;
        
        public override void OnInspectorGUI() {
            var timelineTimer = (TimelineTimerData)target;
        
            // Only enable in play mode
            EditorGUI.BeginDisabledGroup(Application.isPlaying == false);
        
            // Show the time
            var timeSpan = TimeSpan.FromSeconds(timelineTimer.Time);
            EditorGUILayout.LabelField($"Time: {timeSpan:mm\\:ss\\.ff}");
        
            // Show a button to start the timer
            if (GUILayout.Button($"Play (Restart)"))
                timelineTimer.Play();

            _timeToSet = EditorGUILayout.FloatField("Time to set", _timeToSet);
            
            if (GUILayout.Button($"Set time"))
                timelineTimer.SetTime( _timeToSet);
        
            EditorGUI.EndDisabledGroup();
        
            // Refresh the inspector while in play mode
            if (Application.isPlaying) EditorUtility.SetDirty(target);
        }
    }
}
#endif
