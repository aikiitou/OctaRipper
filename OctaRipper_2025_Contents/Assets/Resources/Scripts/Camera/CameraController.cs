using Mono.Cecil.Cil;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    InputSystem_Actions input;

    [Header("プレイヤー")]
    [SerializeField]
    Transform tPlayer;
    [Header("見るところ")]
    [SerializeField]
    Transform tLookTarget;
    [Header("カメラの移動スピード")]
    [SerializeField]
    float fSpeed;
    [Header("視野の最大最小")]
    [SerializeField]
    float fLimitAngle;
    [Header("マウスの感度")]
    [SerializeField]
    float fSens;

    private Vector3 vLookOffset = new Vector3(0, 1f, 0);
    private Vector3 vPosOffset = new Vector3(0, 1, -3);
    private Quaternion qOffsetRot;
    private float fMaxDistance;
    private float fAngleX = 0;
    private float fAngleY = 0;
    
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
        fMaxDistance = vPosOffset.magnitude;
    }

    void FixedUpdate()
    {
        Vector3 rotationOffset = qOffsetRot * vPosOffset;

        transform.position = Vector3.MoveTowards(transform.position, (tPlayer.position + vLookOffset) + rotationOffset, Time.deltaTime * fSpeed);
        tLookTarget.position = Vector3.MoveTowards(tLookTarget.position, (tPlayer.position + vLookOffset), Time.deltaTime * fSpeed);

        transform.LookAt(tLookTarget);
    }

    private void ViewPointMovement(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        input *= fSens;

        fAngleX += input.x;
        fAngleY -= input.y;

        //角度の制限
        fAngleY = Mathf.Clamp(fAngleY, -fLimitAngle, fLimitAngle);

        qOffsetRot = Quaternion.Euler(new Vector3(fAngleY, fAngleX, 0));

        //距離の制限
        Vector3 rayDir = (transform.position - (tPlayer.position + vLookOffset)).normalized;
        if (Physics.Raycast((tPlayer.position + vLookOffset), rayDir, out RaycastHit hitInfo))
        {
            if (hitInfo.transform != transform && hitInfo.transform != tPlayer)
            {
                vPosOffset = Vector3.ClampMagnitude(vPosOffset, hitInfo.distance);
            }
            else
            {
                vPosOffset *= fMaxDistance;
                vPosOffset = Vector3.ClampMagnitude(vPosOffset, fMaxDistance);
            }
        }
        else
        {
            vPosOffset *= fMaxDistance;
            vPosOffset = Vector3.ClampMagnitude(vPosOffset, fMaxDistance);
        }
    }
}
