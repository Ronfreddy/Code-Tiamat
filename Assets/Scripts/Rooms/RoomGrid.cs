using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RoomGrid : MonoBehaviour
{
    // 0 = up, 1 = down, 2 = left, 3 = right
    public GameObject portalPoint;
    public GameObject[] spawnPoints;
    public GameObject[] tunnels;
    public GameObject[] walls;
    public bool[] isTunnelOpen = new bool[4];
    public GameObject[] connectingRooms;
    public List<Vector2> connectingRoomPositions = new List<Vector2>();
    public float generateDoorChance = 100f;
    private float generateDoorChanceDecay = 0.3f;

    public bool OpenTunnel(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                walls[0].SetActive(false);
                tunnels[0].SetActive(true);
                isTunnelOpen[0] = true;
                break;
            case Direction.Down:
                walls[1].SetActive(false);
                tunnels[1].SetActive(true);
                isTunnelOpen[1] = true;
                break;
            case Direction.Left:
                walls[2].SetActive(false);
                tunnels[2].SetActive(true);
                isTunnelOpen[2] = true;
                break;
            case Direction.Right:
                walls[3].SetActive(false);
                tunnels[3].SetActive(true);
                isTunnelOpen[3] = true;
                break;
        }
        generateDoorChance *= generateDoorChanceDecay;
        return true;
    }

    public void BulkSpawnEnemies(int spawnCount)
    {
        if(spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points available to spawn enemies.");
            return;
        }
        for (int i = 0; i < spawnCount; i++)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            LevelManager.instance.SpawnEnemy(spawnPoints[randomSpawnPoint]);
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}