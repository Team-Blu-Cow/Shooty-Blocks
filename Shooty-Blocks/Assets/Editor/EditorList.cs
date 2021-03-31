using UnityEditor;
using UnityEngine;
using System;

[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLabels = 4,
    Buttons = 8,
    Default = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}

public static class EditorList
{
    private static GUIContent
        moveButtonContent = new GUIContent("\u2193", "move down"),
        duplicateButtonContent = new GUIContent("\u2191", "move up"),
        deleteButtonContent = new GUIContent("\u00D7", "delete"),
        addButtonContent = new GUIContent("+", "add group"),
        removeButtonContent = new GUIContent("-", "remove group");

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    public static void Show(SerializedProperty list, SerializedProperty test)
    {
        EditorGUILayout.PropertyField(list);
        for (int i = 0; i < list.arraySize; i++)
        {
            //SerializedProperty difficultyBalance = blockGroup.FindPropertyRelative("m_difficultyBalance");

            ShowElement(list, i, test);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }

        if (GUILayout.Button(removeButtonContent, EditorStyles.miniButton))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(list.arraySize-1);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(list.arraySize-1);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private static void ShowElement(SerializedProperty list, int i, SerializedProperty test)
    {
        SerializedProperty blockGroup = list.GetArrayElementAtIndex(i);
        

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(blockGroup, GUIContent.none);
        ShowButtons(list, i);
        EditorGUILayout.EndHorizontal();

        SerializedObject newObj = new SerializedObject(blockGroup.objectReferenceValue);

        EditorGUILayout.PropertyField(newObj.FindProperty("m_layout"));
        EditorGUILayout.PropertyField(newObj.FindProperty("m_difficultyBalance"));

        EditorGUILayout.Space(10);
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.MoveArrayElement(index, index - 1);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}
