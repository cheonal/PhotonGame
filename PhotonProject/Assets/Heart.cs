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
            Debug.Log("1");
            NetworkManager.networkManager.GetHeart();
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            Player.player.HealthImage.fillAmount += 0.5f;
        }
        if (!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) // 느린쪽에 맞춰서 HIT판정
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
