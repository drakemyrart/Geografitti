using Assets.Scripts.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;

    public Dictionary<int, GameObject> PlayerList = new Dictionary<int, GameObject>();

    public static int myIndex;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();

        ClientHandleData.InitializePackets();
        ClientTCP.InitializingNetworking();
    }

    public void  InstantiatePlayer(int index)
    {
        GameObject go = Instantiate(playerPrefab);
        go.name = "Player:" + index;
        PlayerList.Add(index, go);
    }

    private void OnApplicationQuit()
    {
        ClientTCP.Disconnect();
    }
}
