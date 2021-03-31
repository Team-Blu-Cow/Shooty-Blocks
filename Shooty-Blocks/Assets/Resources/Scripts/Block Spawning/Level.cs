using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Blocks
{
    [CreateAssetMenu(menuName = "My Assets/Level")]
    public class Level : ScriptableObject
    {
        [Tooltip("all the block groups to be spawned. groups are spawned in order of array list from the bottom row of each group to the top.")]
        [SerializeField] public List<BlockGroup> m_level;

        [Tooltip("the number of collectible coins are available in the level")]
        [SerializeField, Range(1,63)] public int m_currencyCount = 1;

        [SerializeField] public BlockGroup testString;

        public void AddNew()
        {
            m_level.Add(new BlockGroup());
        }

        public void AddNew(BlockGroup group)
        {
            m_level.Add(group);
        }

        public void RemoveAt(int index)
        {
            m_level.RemoveAt(index);
        }

        public void Remove(BlockGroup group)
        {
            m_level.Remove(group);
        }
    }

#if UNITY_EDITOR
    /*[CustomPropertyDrawer(typeof(Level))]
    public class LevelPropertyDrawer : PropertyDrawer
    {
        
    }*/

#endif
}
