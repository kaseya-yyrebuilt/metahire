using Photon.Pun;
using TMPro;
using UnityEngine;

/// <summary>
///     控制角色移动、生命、动画
/// </summary>
public class PlayerController : MonoBehaviourPun
{
    public static bool PlayerControlsEnabled { get; set; } = true;

    //===玩家朝向
    private Vector2 _lookDirection = new(1, 0); //默认朝右边
    
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rbody; //刚体组件
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private TextMeshProUGUI _nameTag;
    [SerializeField] private AudioSource _walkAudio;

    private static readonly int _lookXAnimID = Animator.StringToHash("Look X");
    private static readonly int _lookYAnimID = Animator.StringToHash("Look Y");
    private static readonly int _speedAnimID = Animator.StringToHash("Speed");


    private void Start()
    {
        if (!_rbody) _rbody = GetComponent<Rigidbody2D>();
        if (!_anim) _anim = GetComponent<Animator>();
        if (!_walkAudio) _walkAudio = GetComponent<AudioSource>();

        if (photonView.IsMine)
        {
            _nameTag.text = PhotonNetwork.NickName;
            _playerCamera.gameObject.SetActive(true);
        }
        else
        {
            _nameTag.text = photonView.Owner.NickName;
            _playerCamera.gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (!_nameTag) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_nameTag)} is not defined");
        if (!_playerCamera) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_anim)} is not defined");
    }


    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected || !PlayerControlsEnabled)
            return;

        var moveX = Input.GetAxisRaw("Horizontal"); //控制水平移动方向 A：-1 D：1
        var moveY = Input.GetAxisRaw("Vertical");

        var moveVector = new Vector2(moveX, moveY);
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            _lookDirection = moveVector;
            _walkAudio.enabled = true;
        }
        else
        {
            _walkAudio.enabled = false;
        }

        _anim.SetFloat(_lookXAnimID, _lookDirection.x);
        _anim.SetFloat(_lookYAnimID, _lookDirection.y);
        _anim.SetFloat(_speedAnimID, moveVector.magnitude);


        //===移动
        var position = _rbody.position;
        //position.x += moveX * speed * Time.fixedDeltaTime;
        //position.y += moveY * speed * Time.fixedDeltaTime;
        position += moveVector * (_speed * Time.fixedDeltaTime);
        _rbody.MovePosition(position);

    }
}