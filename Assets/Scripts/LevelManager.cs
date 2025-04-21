using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Utilities.Utility;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Difficulty Prefab")]
    public Difficulty currentDifficulty;
    public SO_LevelDifficulty difficultySetting;

    public GameObject[] enemyPrefabs;
    public int enemyKilledCount = 0;
    public List<GameObject> spawnPoints = new List<GameObject>();

    public int currentLevel = 1;
    public GameObject[] levelPrefabs;

    private int currentEnemyCount = 0;
    private int enemyCount = 0;

    public GameObject roomPortal;
    public GameObject portal;

    [Header("Room Generator")]
    public int roomCount = 2;
    public Dictionary<RoomCoordinates, GameObject> roomDictionary = new Dictionary<RoomCoordinates, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    #region Room Generation
    public void GenerateRoom()
    {
        // destroy all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        roomDictionary.Clear();
        GenerateSpawnRoom();

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2 (0, 0);


        // Generate Rooms
        while (roomCount > 0)
        {
            SpreadRoom();
        }

        Debug.Log("Connecting Rooms...");
        // Connect the rooms
        foreach (KeyValuePair<RoomCoordinates, GameObject> currentRoom in roomDictionary)
        {
            Debug.Log("Connecting room: " + currentRoom.Key.x + ", " + currentRoom.Key.y);
            // Connect the rooms here
            // You can use the room coordinates to determine which rooms to connect
            // For example, you can check if there is a room above, below, left, or right of the current room
            RoomCoordinates coord = currentRoom.Key;
            GameObject roomObject = currentRoom.Value;
            RoomGrid roomGrid = roomObject.GetComponent<RoomGrid>();
            // Check every directions
            // Up
            if (CheckGrid(coord.x, coord.y + 1))
            {
                if (!roomGrid.isTunnelOpen[0] && RandomChance(roomGrid.generateDoorChance))
                {
                    roomGrid.OpenTunnel(Direction.Up);
                    roomDictionary[new RoomCoordinates(coord.x, coord.y + 1)].GetComponent<RoomGrid>().OpenTunnel(Direction.Down);
                }
            }
            // Down
            if (CheckGrid(coord.x, coord.y - 1))
            {
                if (!roomGrid.isTunnelOpen[1] && RandomChance(roomGrid.generateDoorChance))
                {
                    roomGrid.OpenTunnel(Direction.Down);
                    roomDictionary[new RoomCoordinates(coord.x, coord.y - 1)].GetComponent<RoomGrid>().OpenTunnel(Direction.Up);
                }
            }
            // Left
            if (CheckGrid(coord.x - 1, coord.y))
            {
                if (!roomGrid.isTunnelOpen[2] && RandomChance(roomGrid.generateDoorChance))
                {
                    roomGrid.OpenTunnel(Direction.Left);
                    roomDictionary[new RoomCoordinates(coord.x - 1, coord.y)].GetComponent<RoomGrid>().OpenTunnel(Direction.Right);
                }
            }
            // Right
            if (CheckGrid(coord.x + 1, coord.y))
            {
                if (!roomGrid.isTunnelOpen[0] && RandomChance(roomGrid.generateDoorChance))
                {
                    roomGrid.OpenTunnel(Direction.Right);
                    roomDictionary[new RoomCoordinates(coord.x + 1, coord.y)].GetComponent<RoomGrid>().OpenTunnel(Direction.Left);
                }
            }
        }


        // Spawn the portal at the furthest room
        GameObject furthestRoom = GetFurthestRoom();
        portal = Instantiate(roomPortal, furthestRoom.GetComponent<RoomGrid>().portalPoint.transform);
        portal.transform.localPosition = Vector3.zero;
        portal.SetActive(false);

        Debug.Log("Room generation complete!");

    }

    private void GenerateSpawnRoom()
    {
        // Generate Spawn Room
        GameObject room = Instantiate(levelPrefabs[0], transform.position, Quaternion.identity);
        room.transform.parent = transform;

        roomDictionary.Add(new RoomCoordinates(0, 0), room);
        roomCount--;
    }

    private void SpreadRoom()
    {
        // Get random room prefab
        GameObject roomPrefab = levelPrefabs[Random.Range(1, levelPrefabs.Length)];
        RoomGrid roomGrid = roomPrefab.GetComponent<RoomGrid>();
        
        // Get one of the existed grid
        List<RoomCoordinates> keys = new List<RoomCoordinates>(roomDictionary.Keys);
        RoomCoordinates randomKey = keys[Random.Range(0, keys.Count)];

        // Get random direction
        int randomDirection = Random.Range(0, 4);
        int x = randomKey.x;
        int y = randomKey.y;
        Direction direction = Direction.Up;
        switch (randomDirection)
        {
            case 0: // Up
                y++;
                direction = Direction.Up;
                break;
            case 1: // Down
                y--;
                direction = Direction.Down;
                break;
            case 2: // Left
                x--;
                direction = Direction.Left;
                break;
            case 3: // Right
                x++;
                direction = Direction.Right;
                break;
        }

        // Check every occupied grid is valid
        bool isEveryGridValid = true;
        Vector2 location = new Vector2(x, y);
        List<Vector2> tempPositions = new List<Vector2>(roomGrid.connectingRoomPositions);

        Debug.Log(roomGrid.connectingRoomPositions.Count + " connecting rooms.");
        for (int i = 0; i < tempPositions.Count; i++)
        {
            tempPositions[i] += location;
        }
        tempPositions.Add(location);

        Debug.Log(tempPositions.Count + " rooms to check.");
        Debug.Log(roomGrid.connectingRoomPositions.Count + " connecting rooms.");
        foreach(Vector2 position in tempPositions)
        {
            if (CheckGrid((int)position.x, (int)position.y))
            {
                isEveryGridValid = false;
                break;
            }
        }
        if (!isEveryGridValid) return;
        Debug.Log(tempPositions.Count + " rooms checked.");


        GameObject room = Instantiate(roomPrefab, transform.position, Quaternion.identity);
        room.transform.parent = transform;
        room.transform.localPosition = new Vector3(x * 20, y * 20);
        roomDictionary.Add(new RoomCoordinates(x, y), room);

        // Connect the rooms
        ConnectRooms(randomKey, new RoomCoordinates(x, y), direction);
        roomCount--;

        // Add additional connecting rooms
        if (roomGrid.connectingRooms.Length == 0) return;
        Debug.Log("Building additional room");

        foreach(Vector2 buildLocation in roomGrid.connectingRoomPositions)
        {
            GameObject builtRoom = Instantiate(roomGrid.connectingRooms[0], transform.position, Quaternion.identity);
            builtRoom.transform.parent = transform;
            builtRoom.transform.localPosition = new Vector3((x + buildLocation.x) * 20, (y + buildLocation.y) * 20);
            roomDictionary.Add(new RoomCoordinates((int)(x + buildLocation.x), (int)(y + buildLocation.y)), builtRoom);
        }
    }

    private void ConnectRooms(RoomCoordinates room1, RoomCoordinates room2, Direction direction)
    {
        // Connect the two rooms here
        // You can use the room coordinates to determine which rooms to connect
        // For example, you can check if there is a room above, below, left, or right of the current room
        GameObject roomObject1 = roomDictionary[room1];
        GameObject roomObject2 = roomDictionary[room2];
        RoomGrid roomGrid1 = roomObject1.GetComponent<RoomGrid>();
        RoomGrid roomGrid2 = roomObject2.GetComponent<RoomGrid>();

        switch (direction)
        {
            case Direction.Up:
                roomGrid1.OpenTunnel(Direction.Up);
                roomGrid2.OpenTunnel(Direction.Down);
                break;
            case Direction.Down:
                roomGrid1.OpenTunnel(Direction.Down);
                roomGrid2.OpenTunnel(Direction.Up);
                break;
            case Direction.Left:
                roomGrid1.OpenTunnel(Direction.Left);
                roomGrid2.OpenTunnel(Direction.Right);
                break;
            case Direction.Right:
                roomGrid1.OpenTunnel(Direction.Right);
                roomGrid2.OpenTunnel(Direction.Left);
                break;
        }
    }

    private bool CheckGrid(int x, int y)
    {
        // Check if the room is already occupied
        return roomDictionary.ContainsKey(new RoomCoordinates(x, y));
    }

    private GameObject GetFurthestRoom()
    {
        float maxDistance = 0;
        GameObject furthestRoom = null;
        foreach (KeyValuePair<RoomCoordinates, GameObject> room in roomDictionary)
        {
            float distance = Vector2.Distance(room.Value.transform.position, Vector2.zero);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestRoom = room.Value;
            }
        }
        return furthestRoom;
    }
    #endregion

    #region Enemies
    public void SpawnEnemies()
    {
        // calculate spawn count
        int extraSpawnCount = Mathf.CeilToInt(difficultySetting.enemySpawnRate.Evaluate(currentLevel));
        int bulkSpawnCount = Mathf.CeilToInt(difficultySetting.fixedEnemyPerRoom.Evaluate(currentLevel));

        foreach (GameObject room in roomDictionary.Values)
        {
            RoomGrid roomGrid = room.GetComponent<RoomGrid>();
            roomGrid.BulkSpawnEnemies(bulkSpawnCount);
        }

        // spawn enemies
        for (int i = 0; i < extraSpawnCount; i++)
        {
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            SpawnEnemy(spawnPoint);
        }
    }

    // Spawn one enemy
    public void SpawnEnemy(GameObject spawnPoint)
    {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyBase>().onDeathEvent += EnemyKilled;
        enemy.GetComponent<EnemyBase>().InitializeStat(currentLevel);

        // increment current enemy count
        currentEnemyCount++;
        enemyCount++;
    }

    public void CheckEnemies()
    {
        float targetEnemyCount = enemyCount * 0.5f;
        // if no enemies left
        if (currentEnemyCount <= targetEnemyCount)
        {
            portal.SetActive(true);
            UIController.Instance.EnablePortalNavigation();
        }
    }

    public void EnemyKilled()
    {
        // decrement current enemy count
        currentEnemyCount--;
        enemyKilledCount++;

        // check enemies
        CheckEnemies();
    }

    // Deprecated: Room Portal will now spawn on room generation
    public void GenerateRoomPortal()
    {
        GameObject[] endpoints = GameObject.FindGameObjectsWithTag("EndPoint");
        GameObject portal = Instantiate(roomPortal, endpoints[Random.Range(0, endpoints.Length)].transform.position, Quaternion.identity);
    }
    #endregion

    #region Progression
    public void StartNewLevel()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            UIController.Instance.FadeOut();
            UIController.Instance.DisablePortalNavigation();
            portal = null;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
            currentEnemyCount = 0;
            enemyCount = 0;
            foreach (GameObject loot in GameObject.FindGameObjectsWithTag("Loot"))
            {
                Destroy(loot);
            }
            foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Destroy(projectile);
            }
        }).
        AppendInterval(1.5f).
        AppendCallback(() =>
        {
            // increment current level
            currentLevel++;

            // clear spawn points list
            spawnPoints.Clear();

            Debug.Log(spawnPoints.Count + " spawn points cleared.");
            roomCount = (int)difficultySetting.roomCurve.Evaluate(currentLevel);

            // generate new room
            GenerateRoom();
            DOVirtual.DelayedCall(0.2f, () => { ResetSpawnPoints(); });
            // spawn enemies
            DOVirtual.DelayedCall(0.2f, () => { SpawnEnemies(); });
        }).
        AppendCallback(() => UIController.Instance.FadeIn()).
        AppendInterval(1.5f).
        AppendCallback(() => UIController.Instance.ShowCurrentLevel(currentLevel));

        sequence.Play();
    }

    private void ResetSpawnPoints()
    {
        GameObject[] spawnPointsArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log(spawnPointsArray.Length + " spawn points found.");
        foreach (GameObject spawnPoint in spawnPointsArray)
        {
            spawnPoints.Add(spawnPoint);
        }
    }

    //Reset the level, not a new level
    public void ResetLevel()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        currentEnemyCount = 0;

        foreach(GameObject loot in GameObject.FindGameObjectsWithTag("Loot"))
        {
            Destroy(loot);
        }
        foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(projectile);
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        UIController.Instance.DisablePortalNavigation();
        roomDictionary.Clear();
        currentLevel = 0;
        enemyKilledCount = 0;
    }

    public void SetDifficulty(SO_LevelDifficulty difficulty)
    {
        // set difficulty
        difficultySetting = difficulty;

        //// set enemy stats
        //foreach (GameObject enemyPrefab in enemyPrefabs)
        //{
        //    enemyPrefab.GetComponent<EnemyBase>().SetDifficulty(difficulty);
        //}
    }
    #endregion
}

// For room coordinates
public struct RoomCoordinates
{
    public int x;
    public int y;

    public RoomCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
