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
    [Header("y方向の視点移動の最小")]
    [SerializeField]
    float limitAngle;
    
    [SerializeField]
    float sens;

    private Vector3 lookOffset = new Vector3(0, 1f, 0);
    private Vector3 posOffset = new Vector3(0, 1, -3);
    private Quaternion offsetRot;
    private float maxDistance;
    private float angleX = 0;
    private float angleY = 0;
    
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
    private void Start()
    {
        maxDistance = posOffset.magnitude;
    }

    void FixedUpdate()
    {
        Vector3 rotationOffset = offsetRot * posOffset;

        transform.position = Vector3.MoveTowards(transform.position, (player.position + lookOffset) + rotationOffset, Time.deltaTime * speed);

        transform.LookAt(player.position + lookOffset);
    }

    private void ViewPointMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        input *= sens;

        angleX += input.x;
        angleY -= input.y;

        //角度の制限
        angleY = Mathf.Clamp(angleY, -limitAngle, limitAngle);

        offsetRot = Quaternion.Euler(new Vector3(angleY, angleX, 0));

        //距離の制限
        Vector3 rayDir = (transform.position - (player.position + lookOffset)).normalized;
        if (Physics.Raycast((player.position + lookOffset), rayDir, out RaycastHit hitInfo))
        {
            if (hitInfo.transform != transform && hitInfo.transform != player)
            {
                posOffset = Vector3.ClampMagnitude(posOffset, hitInfo.distance);
            }
            else
            {
                posOffset *= maxDistance;
                posOffset = Vector3.ClampMagnitude(posOffset, maxDistance);
            }
        }
        else
        {
            posOffset *= maxDistance;
            posOffset = Vector3.ClampMagnitude(posOffset, maxDistance);
        }
    }
}
