using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    InputSystem_Actions input;
    Rigidbody rb;

    Vector3 moveVec = Vector3.zero; //入力からの移動方向を入れる変数

    [Header("プレイヤーの移動")]
    [SerializeField]
    private float speed;
    private void OnEnable()
    {
        input = new InputSystem_Actions();
        input.Enable();
        //インプットシステムに関数の追加
        input.Player.Move.performed += Move;
        input.Player.Move.canceled += Move;
    }
    private void OnDisable()
    {
        input.Disable();
        //インプットシステムに関数の解除
        input.Player.Move.performed -= Move;
        input.Player.Move.canceled -= Move;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        rb.linearVelocity = moveVec;
    }

    private void Move(InputAction.CallbackContext _context)
    {
        Debug.Log(_context.ReadValue<Vector2>());
        Vector2 input = _context.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        moveVec = moveDir * speed;
    }
}
