using UnityEngine;

public class EnemyController01 : EnemyControllerBase
{
    [SerializeField,Header("初期ライフポイント")]
    float fInitLifePoint = 100.0f;

    [SerializeField, Header("最大移動速度")]
    float fMaxSpeed = 2.0f;

    [SerializeField, Header("移動加速度")]
    float fAcceleration = 5.0f;

    [SerializeField, Header("移動自然減速度")]
    float fNaturalBrake = 2.0f;

    [SerializeField, Header("移動減速度")]
    float fBrake = 20.0f;

    [SerializeField, Header("攻撃実行距離")]
    float fAttackDistance = 2.0f;

    [SerializeField, Header("攻撃必要時間")]
    float fAttackActionTime = 1.0f;

    [SerializeField, Header("突進スピード")]
    float fAttackSpeed = 10.0f;

    [SerializeField, Header("与ダメージ")]
    float fAttackDamage = 10.0f;

    [SerializeField, Header("攻撃クールダウン")]
    float fAttackCoolDownTime = 1.5f;

    [SerializeField, Header("見た目回転スピード")]
    float fRotationSpeed = 10.0f;

    [SerializeField, Header("攻撃の当たり判定")]
    GameObject gDamageTrigger; // 攻撃の当たり判定オブジェクト

    bool bIsAcceleration = true; // 現在加速しているかどうか
    bool bIsAttacking = false; // 攻撃しているかどうか
    bool bCanTurn = true; // 回転可能かどうか
    bool bCanAction = true; // 行動可能かどうか
    float fAttackCoolDownTimer;
    float fAttackActionTimer;
    float fFriezeTimer; // 硬直時間
    Vector3 vMoveForce; // 移動量
    GameObject gTargetObject; // 対象のオブジェクト
    Rigidbody rRigidbody; // Rigidbody
    Animator aAnimator; // Animator


    void Start()
    {
        gDamageTrigger.SetActive(false);
        aAnimator = GetComponent<Animator>();
        gTargetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        rRigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        InitializeEnemyData(fInitLifePoint); // 初期ライフポイントのセット
    }

    void Update()
    {
        ChangePattern(); // 行動の切り替え
        Move(); // 動き
        if (bCanTurn)
        {
            Turn(); // 回転
        }
        TimerCountDown(); // タイマー系のカウントダウン
        if (cEnemyLifeController.GetLifePoint <= 0.0f)
        {
            Death(); // 死亡
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(-20.0f, (gameObject.transform.position - gTargetObject.transform.position).normalized * 5.0f, 1.0f);
        }
    }

    void ChangePattern()
    {
        if (fFriezeTimer <= 0.0f)
        {
            bCanAction = true;
        }
        else
        {
            bIsAttacking = false;
            gDamageTrigger.SetActive(false);
            return;
        }
        if (fAttackActionTimer <= 0.0f)
        {
            bIsAttacking = false;
            gDamageTrigger.SetActive(false);
            bCanTurn = true;
        }
        if (((gTargetObject.transform.position - gameObject.transform.position).magnitude <= fAttackDistance ||
            fAttackActionTimer > 0.0f) && bCanAction)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hit, fAttackDistance);
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player" && fAttackCoolDownTimer <= 0.0f)
                {
                    bCanTurn = false;
                    bIsAcceleration = false;
                    Attack();
                }
            }
        }
        if (!bIsAttacking)
        {
            bCanTurn = true;
            bIsAcceleration = true;
        }
    }

    void TimerCountDown()
    {
        if (fAttackCoolDownTimer > 0.0f)
        {
            fAttackCoolDownTimer -= Time.deltaTime;
        }
        if (fAttackActionTimer > 0.0f)
        {
            fAttackActionTimer -= Time.deltaTime;
        }
        if (fFriezeTimer > 0.0f)
        {
            fFriezeTimer -= Time.deltaTime;
        }

    }
    protected override void Move()
    {
        if (bIsAcceleration) // 加速中
        {
            Vector3 moveAddForce = gTargetObject.transform.position - gameObject.transform.position; // 対象と自分の距離算出
            moveAddForce = new Vector3(moveAddForce.x, 0.0f, moveAddForce.z); // y成分を除く
            moveAddForce = moveAddForce.normalized * fAcceleration * Time.deltaTime; // 加速量算出
            vMoveForce -= vMoveForce.normalized * fNaturalBrake * Time.deltaTime; // 摩擦
            vMoveForce += moveAddForce; // 加速度を移動量に加える
            if (vMoveForce.magnitude >= fMaxSpeed) // 上限値矯正
            {
                vMoveForce = vMoveForce.normalized * fMaxSpeed;
            }
        }
        else // ブレーキ
        {
            if (vMoveForce.magnitude >= fBrake * Time.deltaTime)
            {
                vMoveForce -= vMoveForce.normalized * fBrake * Time.deltaTime;
            }
            else
            {
                vMoveForce = Vector3.zero;
            }
        }
        rRigidbody.linearVelocity = new Vector3(vMoveForce.x,rRigidbody.linearVelocity.y,vMoveForce.z);
    }

    protected override void Turn()
    {
        Vector3 horizonDistance = gTargetObject.transform.position - gameObject.transform.position;
        horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.FromToRotation(Vector3.forward, horizonDistance.normalized),
            fRotationSpeed * Time.deltaTime
            ); // 方向転換
    }

    protected override void Attack()
    {
        if (!bIsAttacking)
        {
            gDamageTrigger.SetActive(true);
            Vector3 horizonDistance = gTargetObject.transform.position - gameObject.transform.position;
            horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
            aAnimator.SetTrigger("IsAttacking");
            vMoveForce = horizonDistance.normalized * fAttackSpeed;
            fAttackActionTimer = fAttackActionTime;
            fAttackCoolDownTimer = fAttackCoolDownTime;
            bIsAttacking = true;
        }
    }

    public override void Damage(float _damage, Vector3 _impact, float _friezeTime)
    {
        cEnemyLifeController.ChangeLifePoint(_damage);
        if (_friezeTime > 0)
        {
            KnockBack(_impact, _friezeTime);
        }
    }

    protected override void KnockBack(Vector3 _force, float _friezeTime)
    {
        aAnimator.SetTrigger("Damaged");
        bIsAcceleration = false;
        bCanAction = false;
        vMoveForce = _force;
        fFriezeTimer = _friezeTime;
    }

    protected override void Death()
    {

    }


}
