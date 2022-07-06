using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

/// <summary>
/// 控制角色移动、生命、动画
/// </summary>
public class PlayerController : MonoBehaviourPun
{
    public float speed = 5f;

    public AudioSource RubyWalk;

    public Text nameText;

    public GameObject PlayerCamera;
    //===玩家朝向

    private Vector2 lookDirection = new Vector2(1, 0); //默认朝右边

    public bool state = false; // true(in voice chat), false(not in voice chat)

    public static bool playerControlsEnabled = true;


    Rigidbody2D rbody;//刚体组件
    Animator anim;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        RubyWalk = GetComponent<AudioSource>();

        if (photonView.IsMine)
        {
            nameText.text = PhotonNetwork.NickName;
            PlayerCamera.SetActive(true);
        }
        else
        {
            nameText.text = photonView.Owner.NickName;
            PlayerCamera.SetActive(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        if (playerControlsEnabled)
        {
            float moveX = Input.GetAxisRaw("Horizontal"); //控制水平移动方向 A：-1 D：1
            float moveY = Input.GetAxisRaw("Vertical");

            Vector2 moveVector = new Vector2(moveX, moveY);
            if (moveVector.x != 0 || moveVector.y != 0)
            {
                lookDirection = moveVector;
            }
            //anim.SetFloat("Look X", lookDirection.x);
            //anim.SetFloat("Look Y", lookDirection.y);
            anim.SetFloat("Speed", moveVector.magnitude);


            //===移动
            Vector2 position = rbody.position;
            //position.x += moveX * speed * Time.fixedDeltaTime;
            //position.y += moveY * speed * Time.fixedDeltaTime;
            position += moveVector * speed * Time.fixedDeltaTime;
            rbody.MovePosition(position);


            if (moveX != 0 || moveY != 0)
            {
                RubyWalk.enabled = true;
            }
            else
            {
                RubyWalk.enabled = false;
            }
        }

        //===按下E键进入聊天
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rbody.position, lookDirection, 2f, LayerMask.GetMask("Voice"));
            if (hit.collider != null)
            {
               VoiceTrigger chair = hit.collider.GetComponent<VoiceTrigger>();
                if ((chair != null)&&(state == false))
                {
                    state = true;
                    chair.ShowDialog();//显示进入房间
                }
                else if ((chair != null) && (state == true)){
                    state = false;
                    chair.RemoveDialog();//显示pressE对话框
                }
            }
        }

    }
}
