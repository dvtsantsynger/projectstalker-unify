using UniLayouts.MVP;
using UniLayouts.Views;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Activity))]
public class MvpActivityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIViewGroup root = ((Activity)target).RootView;
        EditorGUILayout.HelpBox("Layout UI Designer", MessageType.Info);
    }
}
