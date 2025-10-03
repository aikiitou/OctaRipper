using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    InputSystem_Actions input;
    Rigidbody rb;

    [Header("プレイヤーの移動のスピード")]
    [SerializeField]
    private float fSpeed;
    [Header("プレイヤーの移動時の傾くスピード")]
    [SerializeField]
    private float fAngleSpeed;
    [Header("プレイヤーの回避時の移動距離")]
    [SerializeField]
    private float fDodgeDistance;
    [Header("回避のクールタイム時間")]
    [SerializeField]
    private float fDodgeCoolTimeValue;

    private Vector3 vMoveVec = Vector3.zero;   //入力からの移動方向を入れる変数
    private Vector3 vInputDir = Vector3.zero;  //入力を記憶
    private Quaternion qMoveRot; //入力からプレイヤーの傾きを入れる変数
    private float fDodgeCoolTime = 0f;
    private bool bIsMove = false;

    private void OnEnable()
    {
        input = new InputSystem_Actions();
        input.Enable();
        //インプットシステムに関数の追加
        input.Player.Move.performed += Move;
        input.Player.Move.canceled += Stop;
        input.Player.Dodge.started += Dodge;
        input.Player.Look.performed += Look;
    }
    private void OnDisable()
    {
        input.Disable();
        //インプットシステムに関数の解除
        input.Player.Move.performed -= Move;
        input.Player.Move.canceled -= Stop;
        input.Player.Dodge.started -= Dodge;
        input.Player.Look.performed -= Look;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if(fDodgeCoolTime > 0f)
        {
            fDodgeCoolTime -= Time.deltaTime;
            if(fDodgeCoolTime <= 0f)
            {
                fDodgeCoolTime = 0f;
            }
        }

        rb.linearVelocity = vMoveVec;

        rb.linearVelocity = vMoveVec * fSpeed + new Vector3(0, rb.linearVelocity.y, 0);

        Vector3 lookDir = new Vector3(vMoveVec.x, 0, vMoveVec.z);
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * fAngleSpeed);
        }
    }

    //移動用の関数
    private void Move(InputAction.CallbackContext _context)
    {
        bIsMove = true;

        vInputDir = _context.ReadValue<Vector2>();

        //カメラの前
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        //カメラの右
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        vMoveVec = cameraForward * vInputDir.y + cameraRight * vInputDir.x;
    }
    //移動を止める
    private void Stop(InputAction.CallbackContext _context)
    {
        //移動用の情報のリセット
        vMoveVec = Vector3.zero;
        vInputDir = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        qMoveRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        bIsMove = false;
    }
    //回避用の関数
    private void Dodge(InputAction.CallbackContext _context)
    {
        if (fDodgeCoolTime <= 0f)
        {
            transform.localPosition += (vMoveVec * fDodgeDistance);

            fDodgeCoolTime = fDodgeCoolTimeValue;
        }
    }
    private void Look(InputAction.CallbackContext _context)
    {
       if(bIsMove == true)
        {
            //カメラの前
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            //カメラの右
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            vMoveVec = cameraForward * vInputDir.y + cameraRight * vInputDir.x;
        }
    }
}
