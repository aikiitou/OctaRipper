using UnityEngine;

public class EnemyController01 : EnemyControllerBase
{
    [SerializeField,Header("初期ライフポイント")]
    float initLifePoint = 100.0f;

    [SerializeField, Header("最大移動速度")]
    float maxSpeed = 2.0f;

    [SerializeField, Header("移動加速度")]
    float acceleration = 5.0f;

    [SerializeField, Header("移動自然減速度")]
    float naturalBrake = 2.0f;

    [SerializeField, Header("移動減速度")]
    float brake = 20.0f;

    [SerializeField, Header("攻撃実行距離")]
    float attackDistance = 2.0f;

    [SerializeField, Header("攻撃必要時間")]
    float attackActionTime = 1.0f;

    [SerializeField, Header("突進スピード")]
    float attackSpeed = 10.0f;

    [SerializeField, Header("与ダメージ")]
    float attackDamage = 10.0f;

    [SerializeField, Header("攻撃クールダウン")]
    float attackCoolDownTime = 1.5f;

    [SerializeField, Header("見た目回転スピード")]
    float rotationSpeed = 10.0f;

    [SerializeField, Header("攻撃の当たり判定")]
    GameObject damageTrigger; // 攻撃の当たり判定オブジェクト

    bool isAcceleration = true; // 現在加速しているかどうか
    bool isAttacking = false; // 攻撃しているかどうか
    bool canTurn = true; // 回転可能かどうか
    bool canAction = true; // 行動可能かどうか
    float attackCoolDownTimer;
    float attackActionTimer;
    float friezeTimer; // 硬直時間
    Vector3 moveForce; // 移動量
    GameObject targetObject; // 対象のオブジェクト
    Rigidbody rb; // RigidBody
    Animator animator; // Animator


    void Start()
    {
        damageTrigger.SetActive(false);
        animator = GetComponent<Animator>();
        targetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        InitializeEnemyData(initLifePoint); // 初期ライフポイントのセット
    }

    void Update()
    {
        ChangePattern(); // 行動の切り替え
        Move(); // 動き
        if (canTurn)
        {
            Turn(); // 回転
        }
        TimerCountDown(); // タイマー系のカウントダウン
        if (enemyLifeController.LifePoint <= 0.0f)
        {
            Death(); // 死亡
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(20.0f, (gameObject.transform.position - targetObject.transform.position).normalized * 5.0f, 1.0f);
        }
    }

    void ChangePattern()
    {
        if (friezeTimer <= 0.0f)
        {
            canAction = true;
        }
        else
        {
            isAttacking = false;
            damageTrigger.SetActive(false);
            return;
        }
        if (attackActionTimer <= 0.0f)
        {
            isAttacking = false;
            damageTrigger.SetActive(false);
            canTurn = true;
        }
        if (((targetObject.transform.position - gameObject.transform.position).magnitude <= attackDistance ||
            attackActionTimer > 0.0f) && canAction)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hit, attackDistance);
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player" && attackCoolDownTimer <= 0.0f)
                {
                    canTurn = false;
                    isAcceleration = false;
                    Attack();
                }
            }
        }
        if (!isAttacking)
        {
            canTurn = true;
            isAcceleration = true;
        }
    }

    void TimerCountDown()
    {
        if (attackCoolDownTimer > 0.0f)
        {
            attackCoolDownTimer -= Time.deltaTime;
        }
        if (attackActionTimer > 0.0f)
        {
            attackActionTimer -= Time.deltaTime;
        }
        if (friezeTimer > 0.0f)
        {
            friezeTimer -= Time.deltaTime;
        }

    }
    protected override void Move()
    {
        if (isAcceleration) // 加速中
        {
            Vector3 moveAddForce = targetObject.transform.position - gameObject.transform.position; // 対象と自分の距離算出
            moveAddForce = new Vector3(moveAddForce.x, 0.0f, moveAddForce.z); // y成分を除く
            moveAddForce = moveAddForce.normalized * acceleration * Time.deltaTime; // 加速量算出
            moveForce -= moveForce.normalized * naturalBrake * Time.deltaTime; // 摩擦
            moveForce += moveAddForce; // 加速度を移動量に加える
            if (moveForce.magnitude >= maxSpeed) // 上限値矯正
            {
                moveForce = moveForce.normalized * maxSpeed;
            }
        }
        else // ブレーキ
        {
            if (moveForce.magnitude >= brake * Time.deltaTime)
            {
                moveForce -= moveForce.normalized * brake * Time.deltaTime;
            }
            else
            {
                moveForce = Vector3.zero;
            }
        }
        rb.linearVelocity = new Vector3(moveForce.x,rb.linearVelocity.y,moveForce.z);
    }

    protected override void Turn()
    {
        Vector3 horizonDistance = targetObject.transform.position - gameObject.transform.position;
        horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.FromToRotation(Vector3.forward, horizonDistance.normalized),
            rotationSpeed * Time.deltaTime
            ); // 方向転換
    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            damageTrigger.SetActive(true);
            Vector3 horizonDistance = targetObject.transform.position - gameObject.transform.position;
            horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
            animator.SetTrigger("IsAttacking");
            moveForce = horizonDistance.normalized * attackSpeed;
            attackActionTimer = attackActionTime;
            attackCoolDownTimer = attackCoolDownTime;
            isAttacking = true;
        }
    }

    public override void Damage(float _damage, Vector3 _impact, float _friezeTime)
    {
        enemyLifeController.ChangeLifePoint(_damage);
        if (_friezeTime > 0)
        {
            KnockBack(_impact, _friezeTime);
        }
    }

    protected override void KnockBack(Vector3 _force, float _friezeTime)
    {
        animator.SetTrigger("Damaged");
        isAcceleration = false;
        canAction = false;
        moveForce = _force;
        friezeTimer = _friezeTime;
    }

    protected override void Death()
    {

    }


}
