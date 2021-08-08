using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(CutSceneModule), true)]
public class CutSceneModuleEditor : Editor
{
    private bool isFolded;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CutSceneModule script = target as CutSceneModule;
        
        if (isFolded = EditorGUILayout.Foldout(isFolded, "Events"))
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnStart"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnEnd"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
