using UnityEngine;
using UnityEditor;

namespace Blocks
{
    // STATIC BLOCK DATA ****************************************************************************************************************************
    public static class BlockData
    {
        public const int Columns = 5;
        public const int MaxRows = 4;
    }

    // BLOCK TYPE ENUM ******************************************************************************************************************************
    // when adding to this, make sure all values are below NumberOfTypes. it is simply used as a length function.
    // LARGE_SUB is used to mark blocks that would be in the space of a large block and works as a flag to prevent them
    // from being rendered and interacted with in the editor.
    public enum BlockType : int
    {
        NONE            = 0,
        DEFAULT         = 1,
        LARGE           = 2,
        LARGE_SUB       = 3,
        NumberOfTypes   = 4
    }

    // BLOCK GRID ARRAY *****************************************************************************************************************************
    // A wrapper class for the array of enums to allow for a custom property drawer to be used in the editor.
    [System.Serializable]
    public class BlockGrid
    {
        [SerializeField] public BlockType[] blocks;

        public BlockGrid(int height)
        {
            blocks = new BlockType[BlockData.Columns * height];
        }

        // resize array without resetting its contents where necessary
        public void Resize(int height)
        {
            BlockType[] temp = blocks;
            blocks = new BlockType[BlockData.Columns * height];

            int range = (blocks.Length > temp.Length) ? temp.Length : blocks.Length;

            for (int i = 0; i < range; i++)
            {
                blocks[i] = temp[i];
            }
        }
    }

    // BLOCK GROUP SCRIPTABLE OBJECT ****************************************************************************************************************
    // A scriptable object to store custom block group configurations.
    [CreateAssetMenu(menuName = "My Assets/Block Group")]
    public class BlockGroup : ScriptableObject
    {
        [SerializeField, Range(1,BlockData.MaxRows)] public int m_height = 3;
        [SerializeField] public BlockGrid m_layout;

        public BlockGroup()
        {
            m_layout = new BlockGrid(m_height);
        }

        // Custom getter and setter for the height parameter to
        // resize layout array
        public int height 
        { 
            get { return m_height; } 
            set { m_height = value; m_layout.Resize(value); }
        }
    }

    // CUSTOM PROPERTY DRAWER ***********************************************************************************************************************
    // A custom class that defines how the BlockGrid class is rendered in the 
    // Unity inspector view.
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(BlockGrid))]
    public class BlockGroupEditor : PropertyDrawer
    {
        // custom style object for inspector
        private GUIStyle BlockColour = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,

            normal = new GUIStyleState()
            {
                background = Texture2D.blackTexture,
                textColor = Color.white
            },

            hover = new GUIStyleState()
            {
                background = Texture2D.blackTexture,
                textColor = Color.white
            },

            active = new GUIStyleState()
            {
                background = Texture2D.blackTexture,
                textColor = Color.white
            }
        };

        // get the height of the property widget in the inspector view.
        // set it to the width of the current inspector window / number of blocks per row
        // multiplied by the number of rows.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ((EditorGUIUtility.currentViewWidth / BlockData.Columns) - 4) * (float)property.FindPropertyRelative("blocks").arraySize / BlockData.Columns;
        }

        // main GUI drawing method
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // de-abstract height of grid
            int height = property.FindPropertyRelative("blocks").arraySize/BlockData.Columns;

            // render each block in grid
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < BlockData.Columns; i++)
                {
                    DrawBlock(i, j, height, position, property);
                }
            }

            EditorGUI.EndProperty();
        }

        // method to draw one block in the inspector window
        private void DrawBlock(int offset,int height,int totalHeight, Rect position, SerializedProperty list)
        {
            // get the property to be rendered
            SerializedProperty property = list.FindPropertyRelative("blocks").GetArrayElementAtIndex(height * BlockData.Columns + offset);

            // Calculate rects used for rendering.
            var block = new Rect(
                position.x + 1 + ((position.width / BlockData.Columns) * offset),
                position.y + ((position.height / totalHeight) * height),
                (position.width / BlockData.Columns) - 2,
                (position.height / totalHeight) - 4);

            var blockLarge = new Rect(
                position.x + 1 + ((position.width / BlockData.Columns) * offset),
                position.y + ((position.height / totalHeight) * height),
                ((position.width / BlockData.Columns) - 2)*2,
                ((position.height / totalHeight) - 4)*2);

            var block_Type = new Rect(
                position.x + 5 + ((position.width / BlockData.Columns) * offset),
                position.y + 2 + ((position.height / totalHeight) * height),
                (position.width / BlockData.Columns) - 10,
                (position.height / totalHeight) - 4);

            var blockLarge_Type = new Rect(
                position.x + 5 + ((position.width / BlockData.Columns) * offset),
                position.y + 2 + ((position.height / totalHeight) * height),
                ((position.width / BlockData.Columns) - 10)*2,
                ((position.height / totalHeight) - 4)*2);

            // initialize scoped members
            Rect button = block_Type;
            bool renderButton = true;
            
            // draw correctly coloured rectangle for block
            // depending on its value
            switch (property.enumValueIndex)
            {
                case (int)BlockType.NONE:
                    EditorGUI.DrawRect(block, new Color(1, 1, 1, 0.05f));
                    break;

                case (int)BlockType.DEFAULT:
                    EditorGUI.DrawRect(block, Color.blue);
                    break;

                case (int)BlockType.LARGE:
                    EditorGUI.DrawRect(blockLarge, Color.red);
                    button = blockLarge_Type;
                    break;

                case (int)BlockType.LARGE_SUB:
                    renderButton = false;
                    break;

            }

            // render button if appropriate
            // if button is pressed cycle through available enum states
            // until valid state is found
            if (renderButton && GUI.Button(button, property.enumDisplayNames[property.enumValueIndex], BlockColour))
            {
                // get current Block state
                int value = property.enumValueIndex;

                // if Block is currently a Large block, reset other
                // blocks it previously covered
                if(value == (int)BlockType.LARGE)
                    SetLargeBlock(false, list, offset, height);

                // increment state
                value++;

                // if Block is attempting to be large, validate area around it
                // to check if block can become large, otherwise increment state.
                if (value == (int)BlockType.LARGE)
                {
                    if (height >= totalHeight - 1 || offset >= BlockData.Columns - 1 || CheckRadiusForLarge(list, offset, height))
                        value++;
                    else
                        SetLargeBlock(true, list, offset, height);
                }

                // skip the LARGE_SUB state as it should only
                // be set to this if another Block covers it.
                if (value == (int)BlockType.LARGE_SUB)
                   value++;

                // if state has reached the end of list, cycle
                // back to start
                if (value >= (int)BlockType.NumberOfTypes)
                {
                    value = 0;
                }

                // set current Block's new state
                property.enumValueIndex = value;
            }

        }

        // A method to check if other large blocks are within
        // a large block zone from inputted Block.
        // returns true if there are.
        private bool CheckRadiusForLarge(SerializedProperty list, int i, int j)
        {
            if (list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j + 1) * BlockData.Columns + i).enumValueIndex == (int)BlockType.LARGE
                || list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j + 1) * BlockData.Columns + i).enumValueIndex == (int)BlockType.LARGE_SUB) return true;
            if (list.FindPropertyRelative("blocks").GetArrayElementAtIndex(j * BlockData.Columns + (i + 1)).enumValueIndex == (int)BlockType.LARGE
                || list.FindPropertyRelative("blocks").GetArrayElementAtIndex(j * BlockData.Columns + (i + 1)).enumValueIndex == (int)BlockType.LARGE_SUB) return true;
            if (list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j + 1) * BlockData.Columns + (i + 1)).enumValueIndex == (int)BlockType.LARGE
                || list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j + 1) * BlockData.Columns + (i + 1)).enumValueIndex == (int)BlockType.LARGE_SUB) return true;
            return false;
        }

        // A method to set/ restore surrounding blocks when a Block becomes Large
        private void SetLargeBlock(bool isLarge, SerializedProperty list, int i, int j)
        {
            if(isLarge)
            {
                list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j+1) * BlockData.Columns + i).enumValueIndex = (int)BlockType.LARGE_SUB;
                list.FindPropertyRelative("blocks").GetArrayElementAtIndex(j * BlockData.Columns + (i+1)).enumValueIndex = (int)BlockType.LARGE_SUB;
                list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j+1) * BlockData.Columns + (i+1)).enumValueIndex = (int)BlockType.LARGE_SUB;
            }
            else
            {
                list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j + 1) * BlockData.Columns + i).enumValueIndex = (int)BlockType.NONE;
                list.FindPropertyRelative("blocks").GetArrayElementAtIndex(j * BlockData.Columns + (i + 1)).enumValueIndex = (int)BlockType.NONE;
                list.FindPropertyRelative("blocks").GetArrayElementAtIndex((j + 1) * BlockData.Columns + (i + 1)).enumValueIndex = (int)BlockType.NONE;
            }
        }
    }
#endif
}


