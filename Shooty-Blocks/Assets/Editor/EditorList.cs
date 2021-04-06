using UnityEditor;
using UnityEngine;
using System;

public static class EditorList
{
    private static GUIContent
        moveButtonContent = new GUIContent("\u2193", "move down"),
        duplicateButtonContent = new GUIContent("\u2191", "move up"),
        deleteButtonContent = new GUIContent("\u00D7", "delete"),
        addButtonContent = new GUIContent("+", "add group"),
        removeButtonContent = new GUIContent("-", "remove group");

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    public static void Show(SerializedProperty list, SerializedProperty difficultyBalance)
    {
        if (list.arraySize > 0)
            DrawListButtons(list, difficultyBalance);

        for (int i = list.arraySize-1; i >= 0; i--)
        {
            ShowElement(list, i, difficultyBalance);
        }

        DrawListButtons(list, difficultyBalance);

        /*EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
            difficultyBalance.arraySize += 1;
        }

        if (GUILayout.Button(removeButtonContent, EditorStyles.miniButton) && list.arraySize > 0)
        {
            int oldSize = list.arraySize;
            difficultyBalance.DeleteArrayElementAtIndex(list.arraySize - 1);
            list.DeleteArrayElementAtIndex(list.arraySize-1);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(list.arraySize-1);
            }
        }
        EditorGUILayout.EndHorizontal();*/
    }

    private static void DrawListButtons(SerializedProperty list, SerializedProperty difficultyBalance)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
            difficultyBalance.arraySize += 1;
        }

        if (GUILayout.Button(removeButtonContent, EditorStyles.miniButton) && list.arraySize > 0)
        {
            int oldSize = list.arraySize;
            difficultyBalance.DeleteArrayElementAtIndex(list.arraySize - 1);
            list.DeleteArrayElementAtIndex(list.arraySize - 1);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(list.arraySize - 1);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private static void ShowElement(SerializedProperty list, int i, SerializedProperty difficultyBalance)
    {
        
        SerializedProperty blockGroup = list.GetArrayElementAtIndex(i);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(blockGroup, GUIContent.none);
        int oldLength = list.arraySize;
        ShowButtons(list, i, difficultyBalance);
        EditorGUILayout.EndHorizontal();

        if (list.arraySize < oldLength)
            return;

        if (list.GetArrayElementAtIndex(i).objectReferenceValue != null || blockGroup.objectReferenceValue != null)
        {
            SerializedObject newObj = new SerializedObject(blockGroup.objectReferenceValue);

            EditorGUILayout.PropertyField(newObj.FindProperty("m_layout"));
            EditorGUILayout.PropertyField(difficultyBalance.GetArrayElementAtIndex(i), new GUIContent("Difficulty Balance"));
        }
        EditorGUILayout.Space(10);
    }

    private static void ShowButtons(SerializedProperty list, int index, SerializedProperty difficultyBalance)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
            difficultyBalance.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.MoveArrayElement(index, index - 1);
            difficultyBalance.MoveArrayElement(index, index - 1);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            difficultyBalance.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}
