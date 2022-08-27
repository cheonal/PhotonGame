using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    int dir;

    void Start()
    {
        Destroy(gameObject, 3.5f);
    }
    void Update()
    {
        transform.Translate(Vector3.right * 7 * Time.deltaTime * dir);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
        if(!PV.IsMine && collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine) // 느린쪽에 맞춰서 HIT판정
        {
            collision.GetComponent<Player>().Hit();
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    void DirRPC(int dir)
    {
        this.dir = dir;
    }
    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
