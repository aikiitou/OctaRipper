using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    const int _FPS = 60;
    const int TIME_LIMIT_FRAME = 10800;
    const int DEATH_EFFECT_FRAME = 12;
    const int REGENE_START_FRAME = 60;
    const float ONE_SECOND_HEAL_VALUE = 50;
    const float SLAH_HEIGHT_OFFSET = 1.0f;

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
    private float fMaxLife;
    [Header("エフェクトの到着フレーム(回避の無敵時間)")]
    [SerializeField]
    private int nEffectArrivalFrame;
    [Header("回避エフェクト")]
    [SerializeField]
    private GameObject gDodgeEffectPrefab;
    [Header("弱溜め攻撃斬撃プレファブ")]
    [SerializeField]
    private GameObject gWeakSlashPrefab;
    [Header("強溜め攻撃斬撃プレファブ")]
    [SerializeField]
    private GameObject gStrongSlashPrefab;
    [Header("回転エフェクト")]
    [SerializeField]
    private GameObject gCircleSlashEffect;
    [Header("右チャージエフェクト")]
    [SerializeField]
    private GameObject gRightChargeEffect;
    [Header("左チャージエフェクト")]
    [SerializeField]
    private GameObject gLeftChargeEffect;
    [Header("スラッシュエフェクト右")]
    [SerializeField]
    private GameObject gRightSlashEffect;
    [Header("スラッシュエフェクト左")]
    [SerializeField]
    private GameObject gLeftSlashEffect;
    [Header("ヒットエフェクト")]
    [SerializeField]
    GameObject gHitEffect;
    [Header("死亡エフェクト")]
    [SerializeField]
    GameObject gDeathEffect;
    [Header("フレームカウンター")]
    [SerializeField]
    private FrameRate cFps;
    [Header("ステージマネジャー")]
    [SerializeField]
    private Transform tStageManager;
    [Header("攻撃の当たり判定")]
    [SerializeField]
    private GameObject[] gAttackColliders;
    [Header("背中の時間制限用オブジェクト")]
    [SerializeField]
    private GameObject[] gBackTimerOjb;

    private GameObject gDodgeEffectInstance;

    private PlayerMoveController cMoveController;
    private PlayerAttackController cAttackController;
    private LifeController cLifeController;

    private bool bIsFreeze = false;
    private bool bIsActive = true;
    private int nTimerFrame = 0;
    private int nBackTimerIndex = 0;
    private int nDodgeRestCoolTimeFrame = 0;
    private int nEffectActiveFrame = 0;
    private int nNoDamageFrame = 0;
    private int nFreezeFrame = 0;
    private int nPastFps;
    private int nFpsDiff;
    private float fOneFrameHealValue;
    private void OnEnable()
    {
        aAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        float fColliderRadius = gameObject.GetComponent<CapsuleCollider>().radius;
        cMoveController = new PlayerMoveController(rb, aAnimator, fSpeed, fAnimSpeed, fDodgeDistance, fAnimSpeed, fColliderRadius);
        cAttackController = new PlayerAttackController(rb, aAnimator, gAttackColliders, gRightChargeEffect, gLeftChargeEffect);
        isInput = new InputSystem_Actions();
        isInput.Enable();
        //インプットシステムに関数の追加
        isInput.Player.Move.performed += Move;
        isInput.Player.Move.canceled += Stop;
        isInput.Player.Dodge.started += Dodge;
        isInput.Player.Look.performed += Look;
        isInput.Player.OnWeakAttack.performed += OnWeakAttackButton;
        isInput.Player.ReleaseWeakAttack.performed += ReleaseWeakAttackButton;
        isInput.Player.OnStrengthAttack.performed += OnStrongAttackButton;
        isInput.Player.ReleaseStrengthAttack.performed += ReleaseStrongAttackButton;
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
        isInput.Player.OnStrengthAttack.performed -= OnStrongAttackButton;
        isInput.Player.ReleaseStrengthAttack.performed -= ReleaseStrongAttackButton;
    }
    private void Start()
    {
        fOneFrameHealValue = ONE_SECOND_HEAL_VALUE / _FPS;

        cLifeController = GetComponent<LifeController>();

        cLifeController.SetLifePoint(fMaxLife);
        cLifeController.SetInvincible(false);

        gDodgeEffectInstance = Instantiate(gDodgeEffectPrefab, transform.parent);

        cAttackController.DisAttackCollider();
    }

    private void Update()
    {
        //FPS計算
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
        
        if(bIsActive == true)
        {
            //クールタイム計算
            if (nDodgeRestCoolTimeFrame > 0)
            {
                nDodgeRestCoolTimeFrame -= nFpsDiff;
                if (nDodgeRestCoolTimeFrame <= 0)
                {
                    nDodgeRestCoolTimeFrame = 0;
                }
            }

            //制限時間
            nTimerFrame += nFpsDiff;
            if (nTimerFrame >= TIME_LIMIT_FRAME / gBackTimerOjb.Length)
            {
                nTimerFrame = 0;
                gBackTimerOjb[nBackTimerIndex].GetComponent<Renderer>().material.SetColor("_BASE_COLOR", Color.black);
                nBackTimerIndex++;
            }
            if (nBackTimerIndex >= gBackTimerOjb.Length)
            {
                Die();
            }

            if (nFreezeFrame > 0)
            {
                nFreezeFrame -= nFpsDiff;
                if (nFreezeFrame <= 0)
                {
                    nFreezeFrame = 0;
                    cLifeController.SetInvincible(false);
                }
            }
            //回復
            nNoDamageFrame += nFpsDiff;
            if(nNoDamageFrame >= 60)
            {
                if (cLifeController.GetLifePoint < fMaxLife)
                {
                    float healValue = fOneFrameHealValue * nFpsDiff;
                    cLifeController.ChangeLifePoint(healValue);
                    if(cLifeController.GetLifePoint >= fMaxLife)
                    {
                        cLifeController.SetLifePoint(fMaxLife);
                    }
                }
            }
            //アタックコントローラー用
            cAttackController.UpdataAttack(nFpsDiff);
        }
        else
        {
            if(nFreezeFrame > 0)
            {
                nFreezeFrame -= nFpsDiff;
                if(nFreezeFrame <= 0)
                {
                    nFreezeFrame = 0;

                    tStageManager.GetComponent<GameOverNotification>().ChageGameOverGameOver();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if(bIsActive == true)
        {
            if (nFreezeFrame <= 0 && bIsFreeze == false)
            {
                cMoveController.FixedUpdateMove(transform);
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
    }

    //移動コントローラー
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
        if(nDodgeRestCoolTimeFrame <= 0 && bIsFreeze == false)
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
    //アタックコントローラー
    private void OnWeakAttackButton(InputAction.CallbackContext _context)
    {
        if(bIsActive == true)
        {
            cAttackController.OnWeakAttackButton();
        }
    }
    private void ReleaseWeakAttackButton(InputAction.CallbackContext _context)
    {
        if (bIsActive == true)
        {
            cAttackController.ReleaseWeakAttackButton();
        }
    }
    private void OnStrongAttackButton(InputAction.CallbackContext _context)
    {
        if (bIsActive == true)
        {
            cAttackController.OnStrongAttackButton();
        }
    }
    private void ReleaseStrongAttackButton(InputAction.CallbackContext _context)
    {
        if (bIsActive == true)
        {
            cAttackController.ReleaseStrongAttackButton();
        }
    }
    private void AnimationStr()
    {
        cAttackController.AnimationStr();
    }
    private void AnimationEnd()
    {
        cAttackController.AnimationEnd();
    }
    private void SetDamage(float _damage)
    {
        cAttackController.SetDamage(_damage);
    }
    private void OnAttackCollider(float _knockBack)
    {
        cAttackController.OnAttackCollider(_knockBack);
    }
    private void DisAttackCollider()
    {
        cAttackController.DisAttackCollider();
    }
    private void DisRightChargeEffect()
    {
        cAttackController.DisRightChargeEffect();
    }
    private void DisBothChargeEffect()
    {
        cAttackController.DisBothChargeEffect();
    }
    //アニメーションに追加する関数
    private void OnFreeze()
    {
        rb.linearVelocity = Vector3.zero;
        bIsFreeze = true;
    }
    private void DisFreeze()
    {
        rb.linearVelocity = Vector3.zero;
        bIsFreeze = false;
    }
    private void GoFront(float _distance)
    {
        rb.AddForce(transform.forward * _distance);
    }
    private void VecLost(float _magnification)
    {
        rb.linearVelocity *= _magnification;
    }
    private void InstantiateWeakSlash()
    {
        GameObject weakSlash = Instantiate(gWeakSlashPrefab,transform.parent);
        Vector3 offset = new Vector3(transform.forward.x, SLAH_HEIGHT_OFFSET, transform.forward.z);
        weakSlash.transform.position = transform.position + offset;
        weakSlash.transform.rotation = transform.rotation;
    }
    private void InstantiateStrongSlash()
    {
        GameObject weakSlash = Instantiate(gStrongSlashPrefab, transform.parent);
        Vector3 offset = new Vector3(transform.forward.x, SLAH_HEIGHT_OFFSET, transform.forward.z);
        weakSlash.transform.position = transform.position + offset;
        weakSlash.transform.rotation = transform.rotation;
    }
    private void OnCircleSlashEffect()
    {
        gCircleSlashEffect.SetActive(true);
    }
    private void DisCircleSlashEffect()
    {
        gCircleSlashEffect.SetActive(false);
    }
    private void OnRightSlashEffect(float _scale)
    {
        gRightSlashEffect.transform.localScale = Vector3.one * _scale;
        gRightSlashEffect.SetActive(true);
    }
    private void DisRightSlashEffect()
    {
        gRightSlashEffect.transform.localScale = Vector3.one;
        gRightSlashEffect.SetActive(false);
    }
    private void OnBothSlashEffect(float _scale)
    {
        gRightSlashEffect.transform.localScale = Vector3.one * _scale;
        gLeftSlashEffect.transform.localScale = Vector3.one * _scale;
        gRightSlashEffect.SetActive(true);
        gLeftSlashEffect.SetActive(true);
    }
    private void DisBothSlashEffect()
    {
        gRightSlashEffect.transform.localScale = Vector3.one;
        gLeftSlashEffect.transform.localScale = Vector3.one;
        gRightSlashEffect.SetActive(false);
        gLeftSlashEffect.SetActive(false);
    }
    //死亡演出
    private void Die()
    {
        cMoveController.Stop(transform);
        bIsActive = false;
        nFreezeFrame = DEATH_EFFECT_FRAME;
        gDeathEffect.SetActive(true);
    }
    //ダメージ関数
    public void Damage(float _damageValue, Vector3 _knockBackVec,int _freezeFrame)
    {
        if(cLifeController.ChangeLifePoint(_damageValue) == true)
        {
            //ダメージ受けてないフレームのリセット
            nNoDamageFrame = 0;
            //ヒットエフェクト
            gHitEffect.GetComponent<HitEffect>().HitParticle();
            if (bIsFreeze == false)
            {
                //硬直・無敵の設定
                nFreezeFrame = _freezeFrame;
                cLifeController.SetInvincible(true);
                //ノックバックベクトルの補正
                _knockBackVec = new Vector3(_knockBackVec.x, 0, _knockBackVec.z);
                //ノックバック
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(_knockBackVec, ForceMode.VelocityChange);
                //攻撃をくらったほうを向く
                transform.rotation = Quaternion.LookRotation(-_knockBackVec);
                //体力0以下なら死亡
                if(cLifeController.GetLifePoint <= 0)
                {
                    Die();
                }
            }
        }        
    }
}