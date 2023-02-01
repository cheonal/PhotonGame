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
        if (collision.tag == "Player")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            NetworkManager.networkManager.HertSpawn();
            Player.player.HealthImage.fillAmount += 0.5f;
            Debug.Log("11");
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
