using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    // BLOCK ROW WRAPPER STRUCT *********************************************************************************************************************
    // A struct to represent a single row of blocks in a level
    struct BlockRow
    {
        public BlockType[] blocks;
        public bool groupEnd;

        public BlockRow(int size)
        {
            blocks = new BlockType[size];
            groupEnd = false;
        }
    }

    // BLOCK SPAWNING CLASS *************************************************************************************************************************
    // The class responsible for loading level data and spawning the blocks
    public class BlockSpawner : MonoBehaviour
    {
        // a queue to store the rows of blocks to spawn
        private Queue<BlockRow> m_level;

        // prefab for the block object 
        private GameObject in_blockPrefab;

        [SerializeField] private Camera m_camera;

        // public alterable variables
        [Tooltip("The speed at which the blocks fall")]
        [SerializeField] private float m_fallSpeed;
        [Tooltip("The space between block groups")]
        [SerializeField, Min(1.15f)] private float m_groupSpacing;
        [Tooltip("The Vertical Space between each row of blocks")]
        [SerializeField, Min(1.15f)] private float m_blockSpacing;

        private void Start()
        {
            in_blockPrefab = Resources.Load<GameObject>("Prefabs/Block");

            BuildLevel(1);
            SpawnRow();
        }

        // load level data from Assets\Resources\Levels\[levelID]
        public static Level LoadLevel(int levelID)
        {
            Level level = Resources.Load<Level>("Levels/" + levelID);

            if(level == null)
                Debug.LogError("Failed to load level: " + levelID);

            return level;
        }

        // append level data to queue of rows
        public void BuildLevel(int levelID)
        {
            // load level from disk
            Level level = LoadLevel(levelID);

            // initialize queue
            m_level = new Queue<BlockRow>();

            // loop through each block group in level
            foreach(BlockGroup group in level.level)
            {
                // loop through each row of block group
                for(int i = group.height-1; i >= 0; i--)
                {
                    // create new row struct
                    BlockRow row = new BlockRow(BlockData.Columns);

                    // assign the appropriate data to each block
                    for (int j = 0; j < BlockData.Columns; j++)
                        row.blocks[j] = group.m_layout.blocks[i * BlockData.Columns + j];

                    // check if this is the last row in the group.
                    // if true, set a flag to signify this
                    if (i == 0)
                        row.groupEnd = true;

                    // push row into queue
                    m_level.Enqueue(row);
                }
            }
        }

        // start spawning the blocks.
        public void StartSpawning()
        {
            if (m_level.Count <= 0)
                return;

            SpawnRow();

        }

        IEnumerator WaitToSpawnNextRow(float time)
        {
            yield return new WaitForSeconds(time);
            SpawnRow();
        }
        
        // a function to spawn a row of blocks
        public void SpawnRow()
        {
            // check that there are still blocks to be spawned
            if(m_level.Count <= 0)
                return;

            // get latest row from the queue
            BlockRow row = m_level.Dequeue();

            // loop through each block
            for(int i = 0; i < BlockData.Columns; i++)
            {
                // calculate the offset
                float offset = 1.0f / ((float)BlockData.Columns * 20.0f);

                // calculate the position on the x axis to spawn the block
                float anchor = (1.0f / (float)BlockData.Columns) * (float)i;

                // convert the screen space coordinate to world space
                Vector3 pos = m_camera.ViewportToWorldPoint(new Vector3(anchor+offset, 1.25f, 0));
                pos = new Vector3(pos.x, pos.y, 0);

                // instantiate new block as necessary
                switch (row.blocks[i])
                {
                    case BlockType.DEFAULT:
                        {
                            GameObject block = Instantiate(in_blockPrefab, pos, Quaternion.identity);
                            block.GetComponent<Block>().hp = Random.Range(5, 15); // TODO @Jay change this to work with difficulty scaling
                            block.GetComponent<Block>().fallSpeed = m_fallSpeed;
                            block.GetComponent<Block>().screenHeight = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
                        }
                        break;

                    case BlockType.LARGE:
                        {
                            GameObject block = Instantiate(in_blockPrefab, pos, Quaternion.identity);
                            block.GetComponent<Block>().hp = Random.Range(5, 15); // TODO @Jay change this to work with difficulty scaling
                            block.GetComponent<Block>().size = 2.15f;
                            block.GetComponent<Block>().fallSpeed = m_fallSpeed;
                            block.GetComponent<Block>().screenHeight = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
                        }
                        break;
                }
            }

            // calculate spacing between this row and the next.
            // space determined depending on whether or not the 
            // next row is in the same group or a new group.
            float spacer = (row.groupEnd) ? m_groupSpacing : 1;

            // calculate the time to wait using the v = dt formula
            StartCoroutine(WaitToSpawnNextRow((m_blockSpacing/m_fallSpeed)*spacer));
            
        }

    }
}
