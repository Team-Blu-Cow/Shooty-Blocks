using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    [CreateAssetMenu(menuName = "My Assets/Level")]
    public class Level : ScriptableObject
    {
        [Tooltip("all the block groups to be spawned. groups are spawned in order of array list from the bottom row of each group to the top.")]
        [SerializeField] public List<BlockGroup> level;
    }
}
