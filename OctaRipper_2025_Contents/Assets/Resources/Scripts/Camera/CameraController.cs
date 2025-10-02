using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    InputSystem_Actions input;

    [Header("カメラを置きたい位置にオブジェクトをおいて入れる")]
    [SerializeField]
    Transform targetPos;
    [Header("プレイヤー")]
    [SerializeField]
    Transform player;
    [Header("カメラの移動,回転スピード")]
    [SerializeField]
    float speed;

    [SerializeField]
    float limitAngle;

    [SerializeField]
    float sens;

    private Vector3 lookOffset = new Vector3(0, 1f, 0);
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        input = new InputSystem_Actions();
        input.Enable();

        //インプットシステムに関数登録
        input.Player.Look.performed += ViewPointMovement;
    }
    private void OnDisable()
    {
        input.Disable();

        //インプットシステムの解除
        input.Player.Look.performed -= ViewPointMovement; 
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position, Time.deltaTime * speed);

        transform.LookAt(player.position + lookOffset);
    }

    private void ViewPointMovement(InputAction.CallbackContext _context)
    {
        //Vector2 input = _context.ReadValue<Vector2>();
        //float angleY = Mathf.Clamp(input.y, -limitAngle, limitAngle);

        //transform.RotateAround(player.position, player.right, angleY);
    }
}
