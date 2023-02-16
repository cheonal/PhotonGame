using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CrossHair : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public LayerMask layerMask;
    public SpriteRenderer dot;
    public Color EnemyCheckColor;
    public Color OriginalColor;
    public Vector3 cc;

    public void ScanTarget(Vector2 vec)
    {
        RaycastHit2D hit = Physics2D.Raycast(vec, transform.position, 1f, layerMask);
        if (hit && !PV.IsMine)
        {
            dot.color = EnemyCheckColor;
        }
        else
        {
            dot.color = OriginalColor;
        }
    }
}
