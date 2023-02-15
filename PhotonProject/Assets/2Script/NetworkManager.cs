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
    public Transform HeartPos; 
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
        PlayerSpawn();
    }


    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Bullet")) Go.GetComponent<PhotonView>().RPC("DestroyRPC", RpcTarget.AllBuffered);
    }

    /// <summary> ĳ���� ���� �Լ� </summary>
    public void PlayerSpawn()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-6f,33f),7,0), Quaternion.identity);
        RespawnPanel.SetActive(false);
    }
    /// <summary> ��Ʈ�� �������� �� </summary>
    public void GetHeart()
    {
        Invoke("HertSpawn", 3f);
    }
    /// <summary> ��Ʈ ��ȯ </summary>
    void HertSpawn()
    {
        PhotonNetwork.Instantiate("Heart", HeartPos.position, Quaternion.identity);
    }

    /// <summary> �������� �������� ��</summary>
    public void GetItem()
    {
        Invoke("ItemSpawn", 1f);
    }
    /// <summary> ������ ��ȯ</summary>

    public void ItemSpawn()
    {
        int ran = Random.Range(0, 5);
        ItemList.Add(ran);
        PhotonNetwork.Instantiate("Item", ItemPos[ran].position, Quaternion.identity);
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
