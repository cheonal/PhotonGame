using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager networkManager;
    public InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject RespawnPanel;

    List<int> ItemList = new List<int>();
    public Transform [] ItemPos;
    public int ItemCount = 2;
    void Awake()
    {
        networkManager = this;
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    }

    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);
        StartCoroutine("DestroyBullet");
        Spawn();
        ItemSpawn();
    }


    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Bullet")) Go.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// 캐릭터 스폰 함수
    /// </summary>
    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-6f,33f),7,0), Quaternion.identity);
        RespawnPanel.SetActive(false);
    }
    /// <summary>
    /// 외부 클래스에서 받아오는 아이템 스폰 함수
    /// </summary>
    public void ItemSpawn()
    {
        int ran = Random.Range(0, 5);
        ItemList.Add(ran);
     //   if (ItemList.Contains(ran))
      //  {
     //       ran = Random.Range(0, 5);
    //    }
    //    else
    //    {
            PhotonNetwork.Instantiate("Item", ItemPos[ran].position, Quaternion.identity);
    //    }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }
}
