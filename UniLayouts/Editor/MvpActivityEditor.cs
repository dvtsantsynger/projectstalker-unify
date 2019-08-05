using UniLayouts.Views;
using UnityEditor;
using UnityEngine;

namespace UniLayouts { 
    [CustomEditor(typeof(MVP.Activity))]
    public class MvpActivityEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UIViewGroup root = ((MVP.Activity)target).RootView;
            EditorGUILayout.HelpBox("Layout UI Designer", MessageType.Info);
        }
    }
}