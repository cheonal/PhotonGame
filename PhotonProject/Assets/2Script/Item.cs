using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Item : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            NetworkManager.networkManager.ItemSpawn();
            Player.player.GetItem();
            
        }
        if (!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) // �����ʿ� ���缭 HIT����
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
