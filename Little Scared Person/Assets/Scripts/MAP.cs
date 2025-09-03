using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] partList;
    public int MapSize = 10;
    private List<GameObject> gameObjects = new List<GameObject>();
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    [Range(0f, 1f)] public float turnChance = 0.3f;     // chance to turn while walking
    [Range(0f, 1f)] public float branchChance = 0.4f;   // chance to add an extra branch

    void Start()
    {
        // Spawn first room at center
        GameObject startRoom = Instantiate(partList[0], Vector3.zero, Quaternion.identity);
        gameObjects.Add(startRoom);
        occupiedPositions.Add(RoundVector(Vector3.zero));

        // Generate until we have enough rooms
        while (gameObjects.Count < MapSize)
        {
            // Pick a random existing room as the branching point
            GameObject origin = gameObjects[Random.Range(0, gameObjects.Count)];
            Vector3 dir = GetRandomDirection();

            // Try spawning the main hallway
            GameObject newRoom = SpawnRoom(origin, dir);
            if (newRoom != null)
            {
                // Walk forward in this direction for a few steps
                WalkHallway(newRoom, dir, Random.Range(2, 6));
            }

            // Maybe add an extra branch
            if (Random.value < branchChance)
            {
                Vector3 branchDir = GetRandomTurn(dir);
                SpawnRoom(origin, branchDir);
            }
        }
    }

    void WalkHallway(GameObject startRoom, Vector3 direction, int length)
    {
        GameObject current = startRoom;
        Vector3 dir = direction;

        for (int i = 0; i < length; i++)
        {
            // Maybe turn
            if (Random.value < turnChance)
                dir = GetRandomTurn(dir);

            GameObject next = SpawnRoom(current, dir);
            if (next == null) break; // blocked
            current = next;
        }
    }

    GameObject SpawnRoom(GameObject originRoom, Vector3 direction)
    {
        GameObject prefab = partList[Random.Range(0, partList.Length)];
        GameObject newRoom = Instantiate(prefab);

        Vector3 originSize = GetRoomSize(originRoom);
        Vector3 newSize = GetRoomSize(newRoom);

        Vector3 offset = Vector3.zero;
        if (direction == Vector3.right || direction == Vector3.left)
        {
            float distance = (originSize.x / 2f) + (newSize.x / 2f);
            offset = direction * distance;
        }
        else if (direction == Vector3.up || direction == Vector3.down)
        {
            if (prefab.CompareTag("long"))
            {
                float distance = (originSize.x / 2f) + (newSize.x / 2f) - 0.8444f;
                offset = direction * distance;
                newRoom.transform.Rotate(0, 0, 90);
            }
            else
            {
                float distance = (originSize.y / 2f) + (newSize.y / 2f);
                offset = direction * distance;
            }
        }

        Vector3 targetPos = originRoom.transform.position + offset;
        Vector3 roundedPos = RoundVector(targetPos);

        // Prevent overlap
        if (occupiedPositions.Contains(roundedPos))
        {
            Destroy(newRoom);
            return null;
        }

        newRoom.transform.position = targetPos;
        gameObjects.Add(newRoom);
        occupiedPositions.Add(roundedPos);

        return newRoom;
    }


    Vector3 GetRoomSize(GameObject room)
    {
        Collider col = room.GetComponentInChildren<Collider>();
        if (col != null) return col.bounds.size;

        Renderer rend = room.GetComponentInChildren<Renderer>();
        if (rend != null) return rend.bounds.size;

        return Vector3.one;
    }

    Vector3 RoundVector(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x * 10f) / 10f,
                           Mathf.Round(v.y * 10f) / 10f,
                           Mathf.Round(v.z * 10f) / 10f);
    }

    Vector3 GetRandomDirection()
    {
        Vector3[] dirs = { Vector3.left, Vector3.right, Vector3.up, Vector3.down };
        return dirs[Random.Range(0, dirs.Length)];
    }

    Vector3 GetRandomTurn(Vector3 currentDir)
    {
        if (currentDir == Vector3.left || currentDir == Vector3.right)
        {
            return Random.value < 0.5f ? Vector3.up : Vector3.down;
        }
        else
        {
            return Random.value < 0.5f ? Vector3.left : Vector3.right;
        }
    }
}
