using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    [CreateAssetMenu(menuName = "My Assets/Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] private List<BlockGroup> m_level;
    }
}
