using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    InputSystem_Actions input;
    Rigidbody rb;

    [Header("プレイヤーの移動のスピード")]
    [SerializeField]
    private float speed;
    [Header("プレイヤーの移動時の傾くスピード")]
    [SerializeField]
    private float angleSpeed;
    //[Header("プレイヤーの移動時の傾き")]
    //[SerializeField]
    //private float angle;

    Vector3 moveVec = Vector3.zero;   //入力からの移動方向を入れる変数
    Vector3 inputDir = Vector3.zero;  //入力を記憶
    Quaternion moveRot; //入力からプレイヤーの傾きを入れる変数
    private bool isMove = false;

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
        rb.linearVelocity = moveVec;

        rb.linearVelocity = moveVec * speed + new Vector3(0, rb.linearVelocity.y, 0);

        Vector3 lookDir = new Vector3(moveVec.x, 0, moveVec.z);
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * angleSpeed);
        }
    }

    //移動用の関数
    private void Move(InputAction.CallbackContext _context)
    {
        isMove = true;

        inputDir = _context.ReadValue<Vector2>();

        //カメラの前
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        //カメラの右
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        moveVec = cameraForward * inputDir.y + cameraRight * inputDir.x;
    }
    //移動を止める
    private void Stop(InputAction.CallbackContext _context)
    {
        //移動用の情報のリセット
        moveVec = Vector3.zero;
        inputDir = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        moveRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        isMove = false;
    }
    //回避用の関数
    private void Dodge(InputAction.CallbackContext _context)
    {
        transform.localPosition += (moveVec / speed);
    }
    private void Look(InputAction.CallbackContext _context)
    {
       if(isMove == true)
        {
            //カメラの前
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            //カメラの右
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            moveVec = cameraForward * inputDir.y + cameraRight * inputDir.x;
        }
    }
}
