using Mono.Cecil.Cil;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    InputSystem_Actions input;

    [Header("プレイヤー")]
    [SerializeField]
    Transform player;
    [Header("カメラの移動スピード")]
    [SerializeField]
    float speed;

    [SerializeField]
    float limitAngle;

    [SerializeField]
    float sens;

    private Vector3 lookOffset = new Vector3(0, 1f, 0);
    private Vector3 posOffset = new Vector3(0, 1, -3);
    
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
        transform.position = Vector3.MoveTowards(transform.position, player.TransformPoint(lookOffset + posOffset), Time.deltaTime * speed);

        transform.LookAt(player.position + lookOffset);
    }

    private void ViewPointMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        //角度の計算
        float angleY = input.y * sens;

        Vector3  newPosOffset = Quaternion.Euler(angleY, 0, 0) * posOffset;
        //制限
        Vector2 offsetZY = new Vector2(newPosOffset.z, newPosOffset.y);
       
        float angle = Vector2.Angle(Vector2.left, offsetZY);
        Debug.Log(angle);
        if(angle < limitAngle)
        {
            posOffset = newPosOffset;
        }
    }
}
