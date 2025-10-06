using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    [Header("回避のクールタイム時間")]
    [SerializeField]
    private float fDodgeCoolTimeValue;
    [Header("アニメーションの速度")]
    [SerializeField]
    private float fAnimSpeed;

    [Header("最大体力")]
    [SerializeField]
    private float fMaxLif;
    [Header("回避エフェクト")]
    [SerializeField]
    private GameObject gDodgeEffectPrefab;

    private GameObject gDodgeEffectInstance;

    PlayerMoveController cMoveController;
    PlayerDodgeEffectController cDodgeEffectController;
    LifeController cLifeController;

    private float fDodgeCoolTime = 0f;
    private void OnEnable()
    {
        aAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        float fColliderRadius = gameObject.GetComponent<CapsuleCollider>().radius;
        cMoveController = new PlayerMoveController(rb, aAnimator, fSpeed, fAnimSpeed, fDodgeDistance, fAnimSpeed, fColliderRadius);
        isInput = new InputSystem_Actions();
        isInput.Enable();
        //インプットシステムに関数の追加
        isInput.Player.Move.performed += Move;
        isInput.Player.Move.canceled += Stop;
        isInput.Player.Dodge.started += Dodge;
        isInput.Player.Look.performed += Look;
    }
    private void OnDisable()
    {
        isInput.Disable();
        //インプットシステムに関数の解除
        isInput.Player.Move.performed -= Move;
        isInput.Player.Move.canceled -= Stop;
        isInput.Player.Dodge.started -= Dodge;
        isInput.Player.Look.performed -= Look;
    }
    private void Start()
    {
        cLifeController = GetComponent<LifeController>();

        cLifeController.SetLifePoint(fMaxLif);
        cLifeController.SetInvincible(false);

        gDodgeEffectInstance = Instantiate(gDodgeEffectPrefab, transform.parent);
        cDodgeEffectController = gDodgeEffectInstance.GetComponent<PlayerDodgeEffectController>();
    }

    private void FixedUpdate()
    {
        cMoveController.FixedUpdateMove(transform);

        if(gDodgeEffectInstance.activeInHierarchy == false)
        {
            gDodgeEffectInstance.transform.position = transform.position;
            gDodgeEffectInstance.transform.rotation = transform.rotation;
        }
        else
        {
            gDodgeEffectInstance.transform.position = Vector3.MoveTowards(gDodgeEffectInstance.transform.position, transform.position, Time.deltaTime * fSpeed);
        }

        if (fDodgeCoolTime > 0f)
        {
            fDodgeCoolTime -= Time.deltaTime;
            if (fDodgeCoolTime <= 0f)
            {
                fDodgeCoolTime = 0f;
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
        if(fDodgeCoolTime <= 0)
        {
            fDodgeCoolTime = fDodgeCoolTimeValue;

            cMoveController.Dodge(transform);
            StartCoroutine(cDodgeEffectController.DodgeEffect());
        }
    }
    private void Look(InputAction.CallbackContext _context)
    {
        cMoveController.Look();
    }

    public void Damage(float _damageValue, Vector3 _knockback)
    {
        cLifeController.ChangeLifePoint(_damageValue);
    }
}