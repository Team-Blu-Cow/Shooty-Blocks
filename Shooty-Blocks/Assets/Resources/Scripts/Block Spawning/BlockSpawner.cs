using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    // BLOCK ROW WRAPPER STRUCT *********************************************************************************************************************
    // A struct to represent a single row of blocks in a level
    internal struct BlockRow
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

        private GameObject in_currencyPrefab;
        private GameObject in_levelEndPrefab;
        private SaveData in_levelSaveData;

        private GameObject m_spawnedInstanceContainer;
        private List<GameObject> m_spawnedInstances;

        private int m_seed;
        private int m_rowNum;
        [SerializeField] private int m_currencyCount;
        [SerializeField] private List<int> m_currencyPositions;

        [SerializeField] private Camera m_camera;
        public Camera cameraBounds
        {
            get{return m_camera;}
            set{m_camera = value;}
        } 

        // public alterable variables
        [Tooltip("The speed at which the blocks fall")]
        [SerializeField] private float m_fallSpeed;

        [Tooltip("The space between block groups")]
        [SerializeField, Min(1.15f)] private float m_groupSpacing;

        [Tooltip("The Vertical Space between each row of blocks")]
        [SerializeField, Min(1.15f)] private float m_blockSpacing;

        [Tooltip("The difficulty of the level")]
        [SerializeField] [Range(0, 10)] private int difficulty;
        private GameObject player;

        private void Start()
        {
            in_blockPrefab = Resources.Load<GameObject>("Prefabs/Block");
            player = GameObject.FindGameObjectWithTag("Player");
            in_currencyPrefab = Resources.Load<GameObject>("Prefabs/Currency Pickup");
            in_levelEndPrefab = Resources.Load<GameObject>("Prefabs/Level End Trigger");

            //BuildLevel(1);
            //StartSpawning();
        }

        // load level data from Assets\Resources\Levels\[levelID]
        public static Level LoadLevel(int levelID)
        {
            Level level = Resources.Load<Level>("Levels/" + levelID);

            if (level == null)
                Debug.LogError("Failed to load level: " + levelID);

            return level;
        }

        // append level data to queue of rows
        public void BuildLevel(int levelID)
        {
            difficulty = levelID;
            // load level from disk
            Level level = LoadLevel(levelID);

            m_spawnedInstanceContainer = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            m_spawnedInstances = new List<GameObject>();

            bool levelHasBeenPlayed = false;
            in_levelSaveData = new SaveData(levelID.ToString(), out levelHasBeenPlayed);

            m_seed = levelID;

            // initialize queue
            m_level = new Queue<BlockRow>();

            // loop through each block group in level
            foreach (BlockGroup group in level.level)
            {
                // loop through each row of block group
                for (int i = group.height - 1; i >= 0; i--)
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

            GenerateCurrencyPositions(level);
        }

        // start spawning the blocks.
        public void StartSpawning()
        {
            if (m_level.Count <= 0)
                return;

            m_rowNum = 0;

            SpawnRow();
        }

        private void GenerateCurrencyPositions(Level level)
        {
            m_currencyCount = level.currencyCount;

            Random.InitState(m_seed);

            m_currencyPositions = new List<int>(m_currencyCount);

            for (int j = 0; j < m_currencyCount; j++)
            {
                m_currencyPositions.Add(-1);
            }

            List<int> usedPositions = new List<int>();

            int totalCount = 0;
            foreach (BlockGroup group in level.level)
            {
                for (int i = group.height - 1; i >= 0; i--)
                {
                    if (!group.HasFreeSpace(i))
                        usedPositions.Add(totalCount + i);
                }

                totalCount += group.height;
            }

            for (int i = 0; i < m_currencyCount; i++)
            {
                int currencyLocation = Random.Range(0, m_level.Count);

                while (usedPositions.Contains(currencyLocation))
                {
                    currencyLocation = Random.Range(0, m_level.Count);
                }

                m_currencyPositions[i] = currencyLocation;
                usedPositions.Add(currencyLocation);
            }
        }

        private IEnumerator WaitToSpawnNextRow(float time)
        {
            yield return new WaitForSeconds(time);
            if (m_level.Count <= 0)
            {
                SpawnLevelEnd();
            }
            else
            {
                m_rowNum++;
                SpawnRow();
            }
            
        }

        // a function to spawn a row of blocks
        public void SpawnRow()
        {
            // check that there are still blocks to be spawned
            if (m_level.Count <= 0)
                return;

            // get latest row from the queue
            BlockRow row = m_level.Dequeue();

            List<int> availableSpaces = new List<int>();
            for (int i = 0; i < BlockData.Columns; i++)
            {
                if (row.blocks[i] == BlockType.NONE)
                    availableSpaces.Add(i);
            }
            int currencySpace = 0;
            if (availableSpaces.Count > 0)
                currencySpace = availableSpaces[Random.Range(0, availableSpaces.Count)];

            // loop through each block
            for (int i = 0; i < BlockData.Columns; i++)
            {
                // calculate the offset
                float offset = 1.0f / ((float)BlockData.Columns * 20.0f);

                // calculate the position on the x axis to spawn the block
                float anchor = (1.0f / (float)BlockData.Columns) * (float)i;

                // convert the screen space coordinate to world space
                if (!m_camera)
                    return;

                Vector3 pos = m_camera.ViewportToWorldPoint(new Vector3(anchor + offset, 1.25f, 0));
                pos = new Vector3(pos.x, pos.y, 0);

                // instantiate new block as necessary
                switch (row.blocks[i])
                {
                    case BlockType.DEFAULT:
                        {
                            GameObject block = Instantiate(in_blockPrefab, pos, Quaternion.identity);
                            block.tag = "Enemy";
                            block.transform.SetParent(m_spawnedInstanceContainer.transform);
                            SetHealth(block);
                            block.GetComponent<Block>().fallSpeed = m_fallSpeed;
                            block.GetComponent<Block>().screenBottom = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
                            block.GetComponent<Block>().screenTop = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 1)).y;
                            block.GetComponentInChildren<BoxCollider2D>().enabled = false;
                            m_spawnedInstances.Add(block);
                        }
                        break;

                    case BlockType.LARGE:
                        {
                            GameObject block = Instantiate(in_blockPrefab, pos, Quaternion.identity);
                            block.tag = "Enemy";
                            block.transform.SetParent(m_spawnedInstanceContainer.transform);
                            SetHealth(block);
                            block.GetComponent<Block>().size = 2.15f;
                            block.GetComponent<Block>().fallSpeed = m_fallSpeed;
                            block.GetComponent<Block>().screenBottom = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
                            block.GetComponent<Block>().screenTop = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 1)).y;
                            block.GetComponentInChildren<BoxCollider2D>().enabled = false;
                            m_spawnedInstances.Add(block);
                        }
                        break;

                    case BlockType.NONE:
                        {
                            if (m_currencyPositions.Contains(m_rowNum) && i == currencySpace)
                            {
                                if (!in_levelSaveData.IsCoinCollected(m_currencyPositions.IndexOf(m_rowNum)))
                                {
                                    GameObject currency = Instantiate(in_currencyPrefab, pos, Quaternion.identity);
                                    currency.transform.SetParent(m_spawnedInstanceContainer.transform);
                                    currency.GetComponent<CurrencyPickup>().fallSpeed = m_fallSpeed;
                                    currency.GetComponent<CurrencyPickup>().screenHeight = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
                                    currency.GetComponent<CurrencyPickup>().in_saveData = in_levelSaveData;
                                    currency.GetComponent<CurrencyPickup>().in_coinId = m_currencyPositions.IndexOf(m_rowNum);
                                    m_spawnedInstances.Add(currency);
                                }
                            }
                        }
                        break;
                }
            }

            // calculate spacing between this row and the next.
            // space determined depending on whether or not the
            // next row is in the same group or a new group.
            float spacer = (row.groupEnd) ? m_groupSpacing : 1;

            // calculate the time to wait using the v = dt formula
            StartCoroutine(WaitToSpawnNextRow((m_blockSpacing / m_fallSpeed) * spacer));
        }
        
        private void SetHealth(GameObject block)
        {
            if (Random.Range(0, 5) != 0)
            {
                block.GetComponent<Block>().hp = Random.Range(5 * (difficulty+1), (5 * (difficulty+1)) * 2);
            }
            else
            {
                block.GetComponent<Block>().hp = Random.Range((5 * (difficulty+1)) * 2, ((5 * (difficulty+1) * 2) * 2));
            }

        }

        private void SpawnLevelEnd()
        {
            // convert the screen space coordinate to world space
            Vector3 pos = m_camera.ViewportToWorldPoint(new Vector3(0.5f,1.1f, 0));
            pos = new Vector3(pos.x, pos.y, 0);

            GameObject endLevelTrigger = Instantiate(in_levelEndPrefab, pos, Quaternion.identity);
            endLevelTrigger.GetComponent<EndLevelTrigger>().fallSpeed = m_fallSpeed;
            endLevelTrigger.GetComponent<EndLevelTrigger>().screenHeight = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
            endLevelTrigger.GetComponent<EndLevelTrigger>().blockSpawner = this;
            endLevelTrigger.GetComponent<EndLevelTrigger>().levelSaveData = in_levelSaveData; 
        }

        public void DestroyAllLevelObjects()
        {
            foreach(GameObject obj in m_spawnedInstances)
            {
                if (obj != null)
                {
                    switch (obj.tag)
                    {
                        case "Enemy":
                            obj.GetComponent<Block>().DestroyFamily();
                            break;

                        case "Currency":
                            obj.GetComponent<CurrencyPickup>().DestroyFamily();
                            break;
                    }
                }

            }
        }

        public void EndLevel()
        {
            DestroyAllLevelObjects();
            in_levelSaveData.WriteToDisk();
            GameController.Instance.userData.WriteToDisk();
        }

        public void SaveLevelData()
        {
            in_levelSaveData.WriteToDisk();
        }
    }
}