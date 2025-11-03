using UnityEngine;

public class RoomScript : MonoBehaviour
{

    public Room roomData;
    private RoomManager roomManager;

    private void Start()
    {
        roomManager = FindAnyObjectByType<RoomManager>(); // Because there is only one roomManager
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) roomManager.OnPlayerEnterRoom(roomData);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) roomManager.OnPlayerExitRoom(roomData);
    }

}