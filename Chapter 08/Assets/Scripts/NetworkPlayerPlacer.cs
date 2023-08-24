using UnityEngine;
using Photon.Pun;

public class NetworkPlayerPlacer : MonoBehaviourPunCallbacks
{
    private GameObject playerInstance;

    private const string PLAYER_PREFAB_NAME = "Network Player";

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        playerInstance = PhotonNetwork.Instantiate(PLAYER_PREFAB_NAME, transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        DespawnPlayer();
    }

    private void DespawnPlayer()
    {
        if (playerInstance)
        {
            PhotonNetwork.Destroy(playerInstance);
        }
    }
}