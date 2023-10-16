using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Player")]
    public string playerPrefabPath;
    public PlayerController[] players;
    public Transform[] spawnPoints;
    public float respawnTime;
    

    private int playersInGame;

    // instance
    public static GameManager instance;

    void Awake()
    {
        instance = this;    
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;

        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    void Start()
    {
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);

        players = new PlayerController[PhotonNetwork.PlayerList.Length];
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        // initialize the player
        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
