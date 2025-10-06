using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController
{
    Animator aAnimator;
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
    [Header("アニメーションの速度")]
    [SerializeField]
    private float fAnimSpeed;

    private Vector3 vMoveVec = Vector3.zero;   //入力からの移動方向を入れる変数
    private Vector3 vInputDir = Vector3.zero;  //入力を記憶
    private Vector3 vCameraForward = Vector3.zero; //カメラからの正面
    private Quaternion qMoveRot; //入力からプレイヤーの傾きを入れる変数
    private float fDodgeCoolTime = 0f;
    private float fAnimBlendX = 0;
    private float fAnimBlendY = 0;
    private bool bIsMove = false;

    public PlayerMoveController(Rigidbody _rb,Animator _animator,float _speed,float _angleSpeed, float _dodgeDistance ,float _dodgeCoolTimeValue,float _animSpeed)
    {
        this.rb = _rb;
        this.aAnimator = _animator;
        this.fSpeed = _speed;
        this.fAngleSpeed = _angleSpeed;
        this.fDodgeDistance = _dodgeDistance;
        this.fDodgeCoolTimeValue = _dodgeCoolTimeValue;
        this.fAnimSpeed = _animSpeed;
    }
    
    public void FixedUpdateMove(Transform _transform)
    {
        if(bIsMove == true)
        {
            fAnimBlendX = Mathf.MoveTowards(fAnimBlendX, vInputDir.x, Time.deltaTime * fAnimSpeed);
            fAnimBlendY = Mathf.MoveTowards(fAnimBlendY, vInputDir.y, Time.deltaTime * fAnimSpeed);

            aAnimator.SetFloat("fMoveDirX", fAnimBlendX);
            aAnimator.SetFloat("fMoveDirY", fAnimBlendY);
        }

        if (fDodgeCoolTime > 0f)
        {
            fDodgeCoolTime -= Time.deltaTime;
            if(fDodgeCoolTime <= 0f)
            {
                fDodgeCoolTime = 0f;
            }
        }

        rb.linearVelocity = vMoveVec;

        rb.linearVelocity = vMoveVec * fSpeed + new Vector3(0, rb.linearVelocity.y, 0);

        Vector3 lookDir = vCameraForward;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(lookDir);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, rot, Time.fixedDeltaTime * fAngleSpeed);
        }
    }

    //移動用の関数
    public void Move(Vector2 inputDir)
    {
        bIsMove = true;
        aAnimator.SetBool("bIsMove", bIsMove);

        vInputDir = inputDir;

        //カメラの前
        vCameraForward = Camera.main.transform.forward;
        vCameraForward.y = 0;
        vCameraForward.Normalize();
        //カメラの右
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        vMoveVec = vCameraForward * vInputDir.y + cameraRight * vInputDir.x;
    }

    //移動を止める
    public void Stop(Transform _transform)
    {
        //移動用の情報のリセット
        vMoveVec = Vector3.zero;
        vInputDir = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        qMoveRot = Quaternion.Euler(0, _transform.eulerAngles.y, 0);
        fAnimBlendX = 0;
        fAnimBlendY = 0;
        bIsMove = false;
        aAnimator.SetBool("bIsMove", bIsMove);
    }

    //回避用の関数
    public IEnumerator Dodge(Transform _transform , Material[] materials)
    {
        if (fDodgeCoolTime <= 0f)
        {
            foreach (Material mat in materials)
            {
                mat.SetFloat("_Rate", 0);
            }

            yield return new WaitForSeconds(0.1f);

            foreach (Material mat in materials)
            {
                mat.SetFloat("_Rate", 1);
            }

            _transform.localPosition += (vMoveVec * fDodgeDistance);

            fDodgeCoolTime = fDodgeCoolTimeValue;
        }
    }

    //カメラの方向が変わったとき、移動していたら向きを変えるよう
    public void Look()
    {
       if(bIsMove == true)
        {
            //カメラの前
            vCameraForward = Camera.main.transform.forward;
            vCameraForward.y = 0;
            vCameraForward.Normalize();
            //カメラの右
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            vMoveVec = vCameraForward * vInputDir.y + cameraRight * vInputDir.x;
        }
    }
}
