using Assets.Maps.Generation;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MapGenerator))]
[CanEditMultipleObjects]
public class GeneratorEditor : Editor
{
    SerializedProperty TerrainOptions;

    void OnEnable()
    {
        TerrainOptions = serializedObject.FindProperty("TerrainOptions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(TerrainOptions);
        serializedObject.ApplyModifiedProperties();
    }
}