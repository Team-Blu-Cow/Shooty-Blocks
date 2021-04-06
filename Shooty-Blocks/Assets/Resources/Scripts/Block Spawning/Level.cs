using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Blocks
{
    [CreateAssetMenu(menuName = "Levels/Level"), System.Serializable]
    public class Level : ScriptableObject
    {
        [Tooltip("all the block groups to be spawned. groups are spawned in order of array list from the bottom row of each group to the top.")]
        [SerializeField] public List<BlockGroup> m_level;

        [Tooltip("the number of collectible coins are available in the level")]
        [SerializeField, Range(1,63)] public int m_currencyCount = 1;

        [SerializeField, Range(-1,1)] public List<float> m_difficultyBalance = new List<float>();

        public void AddNew()
        {
            m_level.Add(new BlockGroup());
            m_difficultyBalance.Add(0f);
        }

        public void AddNew(BlockGroup group)
        {
            m_level.Add(group);
            m_difficultyBalance.Add(0f);
        }

        public void RemoveAt(int index)
        {
            m_level.RemoveAt(index);
            m_difficultyBalance.RemoveAt(index);
        }
    }
}
