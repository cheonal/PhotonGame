using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Vector3 dir;
    public float BulletSpeed;
    Rigidbody2D rigid;
    void Start()
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(transform.up * BulletSpeed, ForceMode2D.Impulse);
        Destroy(gameObject, 3.5f);
    }
    void Update()
    {


        //transform.Translate(dir * BulletSpeed * Time.deltaTime);
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
    void DirRPC(Vector3 dir)
    {
        this.dir = dir;
    }
    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
}
