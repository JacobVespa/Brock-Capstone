using System;
using System.Collections.Generic;
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
    private int roomRotation;

    // Rectangular 2D array: rows x cols = MAP_SIZE x MAP_SIZE
    private Chamber[,] Map;
    private bool[,] occupied;

    private void Start()
    {
        if (MAP_SIZE < 1) MAP_SIZE = 1;
        {
            Map = new Chamber[MAP_SIZE, MAP_SIZE];
            occupied = new bool[MAP_SIZE, MAP_SIZE];
        }

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
        TryToPlaceLargeRoom();
        //TryToPlaceSmallRoom();
    }

    private void TryToPlaceLargeRoom()
    {
        int y = 0;
        while (y < MAP_SIZE)
        {
            int x = 0;
            while (x < MAP_SIZE)
            {
                x += r.Next((int)(MAP_SIZE * 0.1), (int)(MAP_SIZE * 0.4));

                if (CanPlace(x, y)) //error check for out of bounds
                {
                    InitializeRoom(x, y, "Large");
                }
            }
            y += r.Next((int)(MAP_SIZE * 0.1), (int)(MAP_SIZE * 0.4));
        }
    }

    private void TryToPlaceSmallRoom()
    {
        int y = 0;
        while (y < MAP_SIZE)
        {
            int x = 0;
            while (x < MAP_SIZE)
            {
                x += r.Next((int)(MAP_SIZE * 0.1), (int)(MAP_SIZE * 0.2));

                if (x < MAP_SIZE && Map[x, y] == null)
                {
                    InitializeRoom(x, y, "Small ");
                }
            }
            y += r.Next((int)(MAP_SIZE * 0.1), (int)(MAP_SIZE * 0.3));
        }
    }

    private bool CanPlace(int x, int y)
    {
        try { if (Map[x, y] != null) return false; }
        catch (IndexOutOfRangeException) { return false; }

            List<int> openSpaces = new List<int>();

        if (y + 1 < MAP_SIZE && Map[x, y + 1] == null && !occupied[x, y + 1]) openSpaces.Add(0);
        if (x - 1 >= 0 && Map[x - 1, y] == null && !occupied[x - 1, y]) openSpaces.Add(90);
        if (y - 1 >= 0 && Map[x, y - 1] == null && !occupied[x, y - 1]) openSpaces.Add(180);
        if (x + 1 < MAP_SIZE && Map[x + 1, y] == null && !occupied[x + 1, y]) openSpaces.Add(270);

        if (openSpaces.Count > 0)
        {
            roomRotation = openSpaces[r.Next(openSpaces.Count)];
            return true;
        }
        else
            return false;

    }

    private void InitializeRoom(int x, int y, string type) //TODO: determine room type and connections
    {
        Vector3 chamberLocation = Vector3.zero;
        Quaternion chamberRotation = Quaternion.identity;
        GameObject chamberType = null;

        if (type == "Large")
        {
            chamberLocation = new Vector3(x * separationDistance, 0, y * separationDistance);
            chamberRotation = Quaternion.Euler(0, roomRotation, 0);
            chamberType = largeChambers[r.Next(largeChambers.Length)];

            if (roomRotation == 0) occupied[x, y + 1] = true;
            else if (roomRotation == 90) occupied[x - 1, y] = true;
            else if (roomRotation == 180) occupied[x, y - 1] = true;
            else if (roomRotation == 270) occupied[x + 1, y] = true;
            
        }
        else if (type == "Small ")
        {
            chamberLocation = new Vector3(x * separationDistance, 0, y * separationDistance);
            chamberType = smallChambers[r.Next(smallChambers.Length)];

        }

        Map[x, y] = new Chamber(chamberLocation, chamberRotation, chamberType);
        occupied[x, y] = true;
        SpawnRoom(Map[x, y]);
    }

    private void SpawnRoom(Chamber c)
    {
        GameObject room = Instantiate(c.Prefab, c.Location, c.Rotation);
        room.transform.SetParent(transform);
    }

    private class Chamber
    {
        public Vector3 Location { get; set; }
        public Quaternion Rotation { get; set; }
        public GameObject Prefab { get; set; }

        public Chamber(Vector3 location, Quaternion rotation, GameObject prefab)
        {
            Location = location;
            Prefab = prefab;
        }
    }

}