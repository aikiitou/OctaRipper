using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    const int _FPS = 60;

    InputSystem_Actions isInput;
    Animator aAnimator;
    Rigidbody rb;

    //プレイヤームーブコントローラー用変数
    [Header("プレイヤーの移動のスピード")]
    [SerializeField]
    private float fSpeed;
    [Header("プレイヤーの移動時の傾くスピード")]
    [SerializeField]
    private float fAngleSpeed;
    [Header("プレイヤーの回避時の移動距離")]
    [SerializeField]
    private float fDodgeDistance;
    [Header("回避のクールタイムフレーム")]
    [SerializeField]
    private int nDodgeCoolTimeFrame;
    [Header("アニメーションの速度")]
    [SerializeField]
    private float fAnimSpeed;
    [Header("最大体力")]
    [SerializeField]
    private float fMaxLif;
    [Header("回避エフェクト")]
    [SerializeField]
    private GameObject gDodgeEffectPrefab;
    [Header("エフェクトの到着フレーム")]
    [SerializeField]
    private int nEffectArrivalFrame;
    [Header("フレームカウンター")]
    [SerializeField]
    private FrameRate cFps;

    private GameObject gDodgeEffectInstance;

    private PlayerMoveController cMoveController;
    private PlayerAttackController cAttackController;
    private LifeController cLifeController;

    private bool bIsFreeze = false;
    private int nDodgeRestCoolTimeFrame = 0;
    private int nEffectActiveFrame = 0;
    private int nFreezeFrame = 0;
    private int nPastFps;
    private int nFpsDiff;
    private void OnEnable()
    {
        aAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        float fColliderRadius = gameObject.GetComponent<CapsuleCollider>().radius;
        cMoveController = new PlayerMoveController(rb, aAnimator, fSpeed, fAnimSpeed, fDodgeDistance, fAnimSpeed, fColliderRadius);
        cAttackController = new PlayerAttackController(rb, aAnimator);
        isInput = new InputSystem_Actions();
        isInput.Enable();
        //インプットシステムに関数の追加
        isInput.Player.Move.performed += Move;
        isInput.Player.Move.canceled += Stop;
        isInput.Player.Dodge.started += Dodge;
        isInput.Player.Look.performed += Look;
        isInput.Player.OnWeakAttack.performed += OnWeakAttackButton;
        isInput.Player.ReleaseWeakAttack.performed += ReleaseWeakAttackButton;
    }
    private void OnDisable()
    {
        isInput.Disable();
        //インプットシステムに関数の解除
        isInput.Player.Move.performed -= Move;
        isInput.Player.Move.canceled -= Stop;
        isInput.Player.Dodge.started -= Dodge;
        isInput.Player.Look.performed -= Look;
        isInput.Player.OnWeakAttack.performed -= OnWeakAttackButton;
        isInput.Player.ReleaseWeakAttack.performed -= ReleaseWeakAttackButton;
    }
    private void Start()
    {
        cLifeController = GetComponent<LifeController>();

        cLifeController.SetLifePoint(fMaxLif);
        cLifeController.SetInvincible(false);

        gDodgeEffectInstance = Instantiate(gDodgeEffectPrefab, transform.parent);
    }

    private void Update()
    {
        int currentFps = cFps.GetFPS();
        if (currentFps < nPastFps)
        {
            nFpsDiff = _FPS - nPastFps;
            nFpsDiff += currentFps;
            nDodgeRestCoolTimeFrame -= nFpsDiff;
        }
        else
        {
            nFpsDiff = currentFps - nPastFps;
            nDodgeRestCoolTimeFrame -= nFpsDiff;
        }

        nPastFps = currentFps;
        
        //クールタイム計算
        if (nDodgeRestCoolTimeFrame > 0)
        {
            nDodgeRestCoolTimeFrame -= nFpsDiff;
            if (nDodgeRestCoolTimeFrame <= 0)
            {
                nDodgeRestCoolTimeFrame = 0;
            }
        }

        cAttackController.UpdataAttack(nFpsDiff);
    }
    private void FixedUpdate()
    {
        if(nFreezeFrame <= 0 && bIsFreeze == false)
        {
            cMoveController.FixedUpdateMove(transform);
        }
        else
        {
            if(nFreezeFrame > 0)
            {
                nFreezeFrame -= nFpsDiff;
                if (nFreezeFrame <= 0)
                {
                    nFreezeFrame = 0;
                    cLifeController.SetInvincible(false);
                }
            }
        }

        if (gDodgeEffectInstance.activeInHierarchy == false)
        {
            gDodgeEffectInstance.transform.position = transform.position;
            gDodgeEffectInstance.transform.rotation = transform.rotation;
        }
        else
        {
            nEffectActiveFrame += nFpsDiff;
            float Ratio = (float)nEffectActiveFrame / (float)nEffectArrivalFrame;
            gDodgeEffectInstance.transform.position =
                Vector3.Lerp(gDodgeEffectInstance.transform.position, transform.position, Ratio);
            if (Ratio >= 1.0f)
            {
                cLifeController.SetInvincible(false);
                gDodgeEffectInstance.SetActive(false);
            }
        }
    }

    private void Move(InputAction.CallbackContext _context)
    {
        Vector2 input = _context.ReadValue<Vector2>();
        cMoveController.Move(input);   
    }
    private void Stop(InputAction.CallbackContext _context)
    {
        cMoveController.Stop(transform);
    }
    private void Dodge(InputAction.CallbackContext _context)
    {
        if(nDodgeRestCoolTimeFrame <= 0)
        {
            cLifeController.SetInvincible(true);

            nDodgeRestCoolTimeFrame = nDodgeCoolTimeFrame;
            nEffectActiveFrame = nFpsDiff;

            cMoveController.Dodge(transform);

            gDodgeEffectInstance.SetActive(true);
        }
    }
    private void Look(InputAction.CallbackContext _context)
    {
        cMoveController.Look();
    }
    private void OnWeakAttackButton(InputAction.CallbackContext _context)
    {
        cAttackController.OnWeakAttackButton();
    }
    private void ReleaseWeakAttackButton(InputAction.CallbackContext _context)
    {
        cAttackController.ReleaseWeakAttackButton();
    }
    private void AnimationStr()
    {
        cAttackController.AnimationStr();
    }
    private void AnimationEnd()
    {
        cAttackController.AnimationEnd();
    }
    private void OnFreeze()
    {
        rb.linearVelocity = Vector3.zero;
        bIsFreeze = true;
    }
    private void DisFreeze()
    {
        bIsFreeze = false;
    }
    private void GoFront(float distance)
    {
        transform.Translate(transform.forward * distance);
    }
    public void Damage(float _damageValue, Vector3 _knockBackVec,int _freezeFrame)
    {
        if(cLifeController.ChangeLifePoint(_damageValue) == true)
        {
            if (bIsFreeze == true)
            {
                //硬直・無敵の設定
                nFreezeFrame = _freezeFrame;
                cLifeController.SetInvincible(true);
                //ノックバックベクトルの補正
                _knockBackVec = new Vector3(_knockBackVec.x, 0, _knockBackVec.z);
                //ノックバック
                rb.AddForce(_knockBackVec, ForceMode.VelocityChange);
                transform.rotation = Quaternion.LookRotation(-_knockBackVec);
            }
        }        
    }
}