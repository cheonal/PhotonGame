using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Heart : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PV.IsMine)
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        if (!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) // �����ʿ� ���缭 HIT����
        {
            NetworkManager.networkManager.GetHeart();
            collision.GetComponent<Player>().Heal();
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}