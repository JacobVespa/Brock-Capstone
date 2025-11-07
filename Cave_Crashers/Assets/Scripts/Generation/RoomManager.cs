using UnityEngine;
using Random = System.Random;

public class RoomManager : MonoBehaviour
{

    [SerializeField] GameObject S_Chamber01;
    [SerializeField] GameObject S_Chamber02;
    private GameObject[] smallChambers;

    [SerializeField] GameObject L_Chamber01;
    [SerializeField] GameObject L_Chamber02;
    private GameObject[] largeChambers;

    [SerializeField] GameObject Connector01;
    [SerializeField] GameObject Connector02;
    private GameObject[] connectors;

    [SerializeField] int MAP_SIZE = 10;
    private const int separationDistance = 200;
    private Random r = new Random();

    // Rectangular 2D array: rows x cols = MAP_SIZE x MAP_SIZE
    private Chamber[,] Map;


    private void Start()
    {
        if (MAP_SIZE < 1) MAP_SIZE = 1;
        Map = new Chamber[MAP_SIZE, MAP_SIZE];

        FillArray();
        GenerateRooms();
    }

    private void FillArray()
    {
        smallChambers = new GameObject[] { S_Chamber01, S_Chamber02 };
        largeChambers = new GameObject[] { L_Chamber01, L_Chamber02 };
        connectors = new GameObject[] { Connector01, Connector02 };
    }

    private void GenerateRooms()
    {
        // Iterate rows (0) and columns (1)
        for (int x = 0; x < Map.GetLength(0); x++)
        {
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                CreateRoom(x, y);
                SpawnRoom(Map[x, y]);
            }
        }
    }

    private void CreateRoom(int x, int y) // Later determine room type and connections
    {
        // Create a new chamber at a random location
        Vector3 chamberLocation = new Vector3(x * separationDistance, 0, y * separationDistance);

        GameObject chamberType = smallChambers[r.Next(smallChambers.Length)];

        Map[x, y] = new Chamber(chamberLocation, chamberType);
    }

    private void SpawnRoom(Chamber c)
    {
        GameObject room = Instantiate(c.Prefab, c.Location, Quaternion.identity);
        room.transform.SetParent(transform);
    }

    private class Chamber
    {
        public Vector3 Location { get; set; }
        public GameObject Prefab { get; set; }

        public Chamber(Vector3 location, GameObject prefab)
        {
            Location = location;
            Prefab = prefab;
        }
    }

}