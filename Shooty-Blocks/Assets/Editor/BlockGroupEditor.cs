using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Blocks.BlockGroup))]
public class BlockGroupEditor : Editor
{
    private SerializedProperty m_height;
    private SerializedProperty m_layout;

    private int height;

    public void OnEnable()
    {
        m_height = serializedObject.FindProperty("m_height");
        m_layout = serializedObject.FindProperty("m_layout");

        Blocks.BlockGroup group = (Blocks.BlockGroup)target;
        group.height = m_height.intValue;
        height = group.height;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_height);

        EditorGUILayout.PropertyField(m_layout);

        Blocks.BlockGroup group = (Blocks.BlockGroup)target;

        if (height != group.height)
        {
            group.height = m_height.intValue;
            height = group.height;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
