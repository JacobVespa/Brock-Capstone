using UnityEngine;

public class Station : MonoBehaviour, IInteractable
{
    [SerializeField] bool inUse;


    public Transform stationSeat;

    public PlayerController playerController;

    public void Interact()
    {
        Debug.Log("lol");
        SeatPlayer(playerController);
    }

    public void SeatPlayer(PlayerController pc)
    {
        if (inUse) { return; }
        //inUse = true;

        pc.transform.position = stationSeat.position;


    }

}
