using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GridManager))]
[CanEditMultipleObjects]
public class CustomGridManager : Editor
{
    // Properties ----------------
    private SerializedProperty gridWidth;
    private SerializedProperty gridDepth;
    private SerializedProperty delay;
    private SerializedProperty possibleModules;

    private void OnEnable()
    {
        gridWidth = serializedObject.FindProperty("width");
        gridDepth = serializedObject.FindProperty("depth");
        delay = serializedObject.FindProperty("delay");
        possibleModules = serializedObject.FindProperty("possibleModules");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GridManager grid = (GridManager)target;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Set grid details", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(possibleModules);
        EditorGUILayout.PropertyField(gridWidth);
        EditorGUILayout.PropertyField(gridDepth);
        EditorGUILayout.PropertyField(delay);
        if (GUILayout.Button("Generate"))
        {
            grid.RunWFCInEditor();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
