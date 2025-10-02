using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    InputSystem_Actions input;
    Rigidbody rb;

    Vector3 moveVec = Vector3.zero;   //入力からの移動方向を入れる変数
    Quaternion moveRot; //入力からプレイヤーの傾きを入れる変数

    [Header("プレイヤーの移動のスピード")]
    [SerializeField]
    private float speed;
    [Header("プレイヤーの移動時の傾くスピード")]
    [SerializeField]
    private float angleSpeed;
    [Header("プレイヤーの移動時の傾き")]
    [SerializeField]
    private float angle;
    [Header("マウスセンシ")]
    [SerializeField]
    private float sens;
    private void OnEnable()
    {
        input = new InputSystem_Actions();
        input.Enable();
        //インプットシステムに関数の追加
        input.Player.Move.performed += Move;
        input.Player.Move.canceled += Stop;
        input.Player.Dodge.started += Dodge;
        input.Player.Look.performed += ViewPointMovementX;
    }
    private void OnDisable()
    {
        input.Disable();
        //インプットシステムに関数の解除
        input.Player.Move.performed -= Move;
        input.Player.Move.canceled -= Stop;
        input.Player.Dodge.started -= Dodge;
        input.Player.Look.performed -= ViewPointMovementX;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        rb.AddRelativeForce(moveVec);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, moveRot, Time.deltaTime * angleSpeed);
    }

    //移動用の関数
    private void Move(InputAction.CallbackContext _context)
    {
        rb.linearVelocity = Vector3.zero;
        Vector2 input = _context.ReadValue<Vector2>();
        //移動のベクトル生成
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        moveVec = moveDir * speed;

        //傾きの生成
        Vector3 moveAngle = new Vector3(moveDir.z * angle, transform.eulerAngles.y, -moveDir.x * angle);

        moveRot = Quaternion.Euler(moveAngle);
    }
    //移動を止める
    private void Stop(InputAction.CallbackContext _context)
    {
        //移動用の情報のリセット
        moveVec = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        moveRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
    //回避用の関数
    private void Dodge(InputAction.CallbackContext _context)
    {
        transform.localPosition += (moveVec / speed);
    }
    //移動方向を傾ける関数
    private void ViewPointMovementX(InputAction.CallbackContext _context)
    {
        //マウスの入力によって回転させる
        Vector2 input = _context.ReadValue<Vector2>();
        transform.Rotate(0, input.x * sens, 0);

        //傾き方向を保管
        Vector3 newAngle = moveRot.eulerAngles;
        newAngle.y = transform.eulerAngles.y;
        moveRot = Quaternion.Euler(newAngle);
    }
}
