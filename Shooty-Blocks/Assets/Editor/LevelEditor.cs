using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Blocks.Level)), CanEditMultipleObjects]
public class LevelEditor : Editor
{
    private SerializedProperty m_level;
    private SerializedProperty m_currencyCount;
    private SerializedProperty m_test;

    private int levelLength;

    public void OnEnable()
    {
        m_level = serializedObject.FindProperty("m_level");
        m_currencyCount = serializedObject.FindProperty("m_currencyCount");
        m_test = serializedObject.FindProperty("testString");

        levelLength = m_level.arraySize;
    }

    public override void OnInspectorGUI()
    {
        /*EditorGUILayout.PropertyField(m_height);

        EditorGUILayout.PropertyField(m_layout);

        Blocks.BlockGroup group = (Blocks.BlockGroup)target;

        if (height != group.height)
        {
            group.height = m_height.intValue;
            height = group.height;
        }*/

        serializedObject.Update();

        EditorGUILayout.PropertyField(m_currencyCount);
        EditorGUILayout.PropertyField(m_test);

        levelLength = m_level.arraySize;
        EditorList.Show(m_level, m_test);
        
        serializedObject.ApplyModifiedProperties();
    }
}
