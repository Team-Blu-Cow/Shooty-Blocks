using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Blocks
{
    // BLOCK ROW WRAPPER STRUCT *********************************************************************************************************************
    // A struct to represent a single row of blocks in a level
    internal struct BlockRow
    {
        public BlockType[] blocks;
        public bool groupEnd;
        [Range(-1, 1)] public float difficultyBalance;

        public BlockRow(int size)
        {
            blocks = new BlockType[size];
            groupEnd = false;
            difficultyBalance = 0f;
        }
    }

    // BLOCK SPAWNING CLASS *************************************************************************************************************************
    // The class responsible for loading level data and spawning the blocks
    public class BlockSpawner : MonoBehaviour
    {
        // a queue to store the rows of blocks to spawn
        private Queue<BlockRow> m_level;

        // loaded prefabs
        private GameObject in_blockPrefab;
        private GameObject in_currencyPrefab;
        private GameObject in_levelEndPrefab;

        // reference to level save data
        private SaveData in_levelSaveData;

        // scene and data containers for all spawned instances
        private GameObject m_spawnedInstanceContainer;
        private List<GameObject> m_spawnedInstances;

        // seed used for random generation
        private int m_seed;

        // count of current row being spawned
        private int m_rowNum;

        // flag to store whether the level has been frozen
        private bool m_frozen;

        // the amount of currency contained within the level
        [SerializeField] private int m_currencyCount;
        public int CurrencyCount
        {
            get { return m_currencyCount; }
        }

        // list of positions currency will be spawned
        [SerializeField] private List<int> m_currencyPositions;

        // the bounds of the viewport
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

        [Tooltip("The colors of the enemies")]
        [SerializeField] private Sprite[] colors;
        [SerializeField] public Color[] textColors;

        public void OnLevelFreeze(bool state)
        {
            m_frozen = state;
        }

        private void Start()
        {
            in_blockPrefab = Resources.Load<GameObject>("Prefabs/Block");
            player = GameObject.FindGameObjectWithTag("Player");
            in_currencyPrefab = Resources.Load<GameObject>("Prefabs/Currency Pickup");
            in_levelEndPrefab = Resources.Load<GameObject>("Prefabs/Level End Trigger");
            colors = Resources.LoadAll<Sprite>("Sprites/Enemies");
            m_frozen = false;
            GameController.Instance.freezeDelegate += OnLevelFreeze;
        }

        // load level data from Assets\Resources\Levels\[levelID]
        public static Level LoadLevel(int levelID)
        {
            Level level = Resources.Load<Level>("Levels/" + levelID);

            if (level == null)
                Debug.LogWarning("Failed to load level: " + levelID);
            
            return level;
        }

        // append level data to queue of rows
        public void BuildLevel(int levelID)
        {
            difficulty = levelID;
            // load level from disk
            Level level = LoadLevel(levelID);

            // initialize instance containers
            m_spawnedInstanceContainer = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
            m_spawnedInstances = new List<GameObject>();

            // load save data
            bool levelHasBeenPlayed = false;
            in_levelSaveData = new SaveData(levelID.ToString(), out levelHasBeenPlayed);

            //set seed
            m_seed = levelID;

            // initialize queue
            m_level = new Queue<BlockRow>();

            // loop through each block group in level
            int k = 0;
            foreach (BlockGroup group in level.m_level)
            {
                // loop through each row of block group
                for (int i = group.height - 1; i >= 0; i--)
                {
                    // create new row struct
                    BlockRow row = new BlockRow(BlockData.Columns);
                    // assign the appropriate data to each block
                    for (int j = 0; j < BlockData.Columns; j++)
                        row.blocks[j] = group.m_layout.blocks[i * BlockData.Columns + j];

                    row.difficultyBalance = level.m_difficultyBalance[k];

                    // check if this is the last row in the group.
                    // if true, set a flag to signify this
                    if (i == 0)
                        row.groupEnd = true;

                    // push row into queue
                    m_level.Enqueue(row);
                }
                k++;
            }

            m_currencyCount = level.m_currencyCount;

            if (GameController.Instance.userData.controlGroup)
                GenerateCurrencyPositions(level);
        }

        // start spawning the blocks.
        public void StartSpawning()
        {
            if (m_level.Count <= 0)
                return;

            DestroyAllLevelObjects();

            m_rowNum = 0;

            SpawnRow();
        }

        // generate the positions of the currency objects
        private void GenerateCurrencyPositions(Level level)
        {
            // init rng using seed
            Random.InitState(m_seed);

            // initialize positions list
            m_currencyPositions = new List<int>(m_currencyCount);

            // fill positions in list with dummy values
            for (int j = 0; j < m_currencyCount; j++)
            {
                m_currencyPositions.Add(-1);
            }

            // temp list to store used rows
            List<int> usedPositions = new List<int>();

            // loop through all groups checking if they have a free space 
            // for currency to spawn
            int totalCount = 0;
            foreach (BlockGroup group in level.m_level)
            {
                for (int i = group.height - 1; i >= 0; i--)
                {
                    if (!group.HasFreeSpace(i))
                        usedPositions.Add(totalCount);
                    totalCount++;
                }
            }

            // generate positions using the remaining available rows
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
            float timeVal = 0f;

            while(true)
            {
                if (!m_frozen)
                    timeVal += Time.deltaTime;

                if (timeVal >= time)
                    break;

               yield return null;
            }

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

            // create list of all available spaces in this row
            List<int> availableSpaces = new List<int>();
            for (int i = 0; i < BlockData.Columns; i++)
            {
                if (row.blocks[i] == BlockType.NONE)
                    availableSpaces.Add(i);
            }
            // generate currency spawn position
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

                // instantiate new prefab
                switch (row.blocks[i])
                {
                    case BlockType.DEFAULT:
                    case BlockType.LARGE:
                        {
                            GameObject block = CreateBlock(pos, row.blocks[i]);
                            SetHealth(block, row.difficultyBalance, i);
                        }
                        break;
                    case BlockType.INDESTRUCTABLE:
                        {
                            CreateBlock(pos, row.blocks[i]);
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

        private GameObject CreateBlock(Vector3 pos, BlockType type)
        {
            // create instance of block prefab
            GameObject block = Instantiate(in_blockPrefab, pos, Quaternion.identity);

            // set object tag
            block.tag = "Enemy";

            // contain object within parent object to keep
            // scene hierarchy tidy
            block.transform.SetParent(m_spawnedInstanceContainer.transform);

            // set type Dependant variables
            block.GetComponent<Block>().type = type;
            if(type == BlockType.LARGE) 
                block.GetComponent<Block>().size = 0.8f;

            if (type == BlockType.INDESTRUCTABLE)
            {
                block.GetComponent<Block>().text = "\u221E";
                block.GetComponentInChildren<SpriteRenderer>().sprite = colors[0];
                block.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
            else
            {
                //generate colors
                // TODO: this needs to be re factored to fit new brief
                int rand = Random.Range(1, colors.Length-1);
                block.GetComponentInChildren<SpriteRenderer>().sprite = colors[rand];
                block.GetComponentInChildren<TextMeshPro>().color = textColors[rand];
            }

            // set universal variables
            block.GetComponent<Block>().fallSpeed = m_fallSpeed;
            block.GetComponent<Block>().screenBottom = m_camera.ViewportToWorldPoint(new Vector3(1, 0, 1)).y;
            block.GetComponent<Block>().screenTop = m_camera.ViewportToWorldPoint(new Vector3(1, 1, 1)).y;

            // temporarily disable collider on block
            block.GetComponentInChildren<Collider2D>().enabled = false;

            // instantiate block with correct freeze value
            block.GetComponent<Block>().frozen = m_frozen;

            // listen for freeze event
            GameController.Instance.freezeDelegate += block.GetComponent<Block>().OnLevelFreeze;

            // add block to list of spawned instances
            m_spawnedInstances.Add(block);

            return block;
        }

        private void SetHealth(GameObject block, float n, int pos)
        {
            int blockHp;

            // calculate hp based on global difficulty
            blockHp = Random.Range(5 * Mathf.RoundToInt(1 + difficulty), (5 * Mathf.RoundToInt(1 + (difficulty * 0.75f))) * 2);

            // calculate hp based on local row difficulty balance
            // go to https://www.desmos.com/calculator/l9fsmqno2w 
            // to play with the equation and see how it works
            Vector2 p1 = new Vector2(0, 0.5f);
            Vector2 p2 = new Vector2(BlockData.Columns - 1, 2);

            float m = ((p2.y - p1.y) * n) / (p2.x - p1.x);
            float b = (-0.75f * n) + (1 + (0.25f * Mathf.Abs(n)));

            float difficultyMod = (m * pos) + b;
            blockHp = Mathf.RoundToInt((float)blockHp * difficultyMod);

            // scale hp based on block size
            blockHp *= ((block.GetComponent<Block>().type == BlockType.LARGE) ? 2 : 1);

            // randomly select blocks to have doubled hp
            blockHp *= (Random.Range(0, 5) != 0) ? 1 : 2;

            block.GetComponent<Block>().hp = blockHp;

            block.GetComponent<Block>().ChangeColor();
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

        private void OnDestroy()
        {
            GameController.Instance.freezeDelegate -= OnLevelFreeze;
        }
    }
}