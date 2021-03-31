using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Blocks.Level)), CanEditMultipleObjects]
public class LevelEditor : Editor
{
    private SerializedProperty m_level;
    private SerializedProperty m_currencyCount;
    private SerializedProperty m_difficultyBalance;

    private int levelLength;

    public void OnEnable()
    {
        m_level = serializedObject.FindProperty("m_level");
        m_currencyCount = serializedObject.FindProperty("m_currencyCount");
        m_difficultyBalance = serializedObject.FindProperty("m_difficultyBalance");

        levelLength = m_level.arraySize;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_currencyCount);
        EditorGUILayout.Space(10);

        levelLength = m_level.arraySize;
        EditorList.Show(m_level, m_difficultyBalance);
        
        serializedObject.ApplyModifiedProperties();
    }
}
