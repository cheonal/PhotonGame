using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Trap : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().Hit();
            Player.player.PlayerSpeedCheck(-0.5f);
            Invoke("TrapOut", 1f);
        }
    }
    void TrapOut()
    {
        Player.player.PlayerSpeedCheck(+0.5f);
    }
}
