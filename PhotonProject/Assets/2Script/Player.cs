using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D rigid;
    public Animator anim;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;
    public Image HealthImage;

    public Transform FirePoint;
    public Vector3 Playerdir;
    Camera cam;

    public int JumpCount;
    bool isGround;
    Vector3 curPos;

    void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;

        if (PV.IsMine)
        {
            // 2D ī�޶�
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;  
        }
    }
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        Playerdir = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (PV.IsMine)
        {
            float axis = Input.GetAxisRaw("Horizontal");
            rigid.velocity = new Vector2(4 * axis, rigid.velocity.y);

            if (axis != 0)
            {
                anim.SetBool("walk", true);
                PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); //�����ӽ� flipx�� ����ȭ ���ֱ� ���ؼ� allbuffered ���ҽ� �����ӽ� x�� �ȸ��� 
            }
            else
            {
                anim.SetBool("walk", false);
            }

            //���� �ٴ�üũ
            isGround = Physics2D.OverlapCircle
                ((Vector2)transform.position + new Vector2(0, -0.5f), 0.07f, 1 << LayerMask.NameToLayer("Ground"));
            anim.SetBool("jump", !isGround);
            if (isGround)
                JumpCount = 2;
            if (Input.GetKeyDown(KeyCode.Space) && JumpCount > 0)
            {
                JumpCount--;
                PV.RPC("JumpRPC", RpcTarget.All);
            }

            //�Ѿ� �߻�
            if (Input.GetMouseButtonDown(0))
            {
                if (Playerdir.x < 0)
                    SR.flipX = true;
                else
                    SR.flipX = false;

                PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(SR.flipX ? -0.4f : 0.4f, -0.11f, 0), Quaternion.identity)
                 .GetComponent<PhotonView>().RPC("DirRPC", RpcTarget.All, Playerdir);
                anim.SetTrigger("shot");
            }

        }
        //�ε巴�� ��ġ����ȭ 
        else if ((transform.position - curPos).sqrMagnitude >= 100) //���� ������� �����̵��ϴ� ������ ��ġ ����
        {
            transform.position = curPos;
        }
        else // ���� ����� �ƴ϶�� �ε巴�� ����   Vector3.Lerp =�ε巴�� ����
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        }
        
            
    }
    [PunRPC]
    void FlipXRPC(float axix)
    {
        SR.flipX = axix == -1;
    }
    [PunRPC] 
    void JumpRPC()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * 700);
    }
    public void Hit()
    {
        HealthImage.fillAmount -= 0.1f;
        if(HealthImage.fillAmount <= 0)
        {
            GameObject.Find("Canvas").transform.Find("ReSpawnPannel").gameObject.SetActive(true);
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered); //AllBuffered�� �ؾ� ����� ����� ������װ� �ȵȴ�
            //DestroyRPC�� �÷��̾�� �Ѿ��̵� AllBuffered�� �ؾ���
        }
    }
    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(HealthImage.fillAmount);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            HealthImage.fillAmount = (float)stream.ReceiveNext();
        }
    }
}
