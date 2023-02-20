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
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) // �����ʿ� ���缭 HIT����
        {
            collision.GetComponent<Player>().SpeedUp();
            NetworkManager.networkManager.GetItem();
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
