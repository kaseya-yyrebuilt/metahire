using Photon.Pun;
using UnityEngine;

/// <summary>
/// 控制角色移动、生命、动画
/// </summary>
public class PlayerController : MonoBehaviourPun
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private AudioSource walkAudio;

    [SerializeField]
    private TMPro.TextMeshProUGUI nameTag;

    [SerializeField]
    private GameObject PlayerCamera;
    //===玩家朝向

    private Vector2 lookDirection = new Vector2(1, 0); //默认朝右边

    [SerializeField]
    private bool state = false; // true(in voice chat), false(not in voice chat)

    public static bool playerControlsEnabled { get; set; } = true;


    [SerializeField]
    private Rigidbody2D rbody;//刚体组件
    [SerializeField]
    private Animator anim;

    void Start()
    {
        if (!rbody) rbody = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
        if (!walkAudio) walkAudio = GetComponent<AudioSource>();

        if (photonView.IsMine)
        {
            nameTag.text = PhotonNetwork.NickName;
            PlayerCamera.SetActive(true);
        }
        else
        {
            nameTag.text = photonView.Owner.NickName;
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
                walkAudio.enabled = true;
            }
            else
            {
                walkAudio.enabled = false;
            }
            anim.SetFloat("Look X", lookDirection.x);
            anim.SetFloat("Look Y", lookDirection.y);
            anim.SetFloat("Speed", moveVector.magnitude);


            //===移动
            Vector2 position = rbody.position;
            //position.x += moveX * speed * Time.fixedDeltaTime;
            //position.y += moveY * speed * Time.fixedDeltaTime;
            position += moveVector * speed * Time.fixedDeltaTime;
            rbody.MovePosition(position);
        }
    }
}
