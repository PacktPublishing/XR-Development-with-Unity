using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string ROOM_NAME = "Multiplayer Room";
    private const byte MAX_PLAYERS = 5;

    private void Awake()
    {
        InitiateServerConnection();
    }

    private void InitiateServerConnection()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Attempting server connection...");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master Server.");

        JoinOrCreateGameRoom();
    }

    private void JoinOrCreateGameRoom()
    {
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = MAX_PLAYERS,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Successfully joined a room.");
    }

    public override void OnPlayerEnteredRoom(Player newParticipant)
    {
        base.OnPlayerEnteredRoom(newParticipant);
        Debug.Log("Another player has joined the room.");
    }

}