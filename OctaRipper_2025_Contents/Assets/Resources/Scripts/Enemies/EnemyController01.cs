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

    [SerializeField, Header("攻撃クールダウン")]
    float attackCoolDownTime = 1.5f;

    [SerializeField, Header("見た目回転スピード")]
    float rotationSpeed = 360.0f;

    bool isAcceleration = true; // 現在加速しているかどうか
    bool isAttacking = false; // 攻撃しているかどうか
    bool canTurn = true; // 回転可能かどうか
    float attackCoolDownTimer;
    float attackActionTimer;
    MovePattern currentPattern; // 現在の状態
    Vector3 moveForce; // 移動量
    GameObject targetObject; // 対象のオブジェクト
    Rigidbody rb; // RigidBody


    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        InitializeEnemyData(initLifePoint); // 初期ライフポイントのセット
        currentPattern = MovePattern.Move;
    }

    void Update()
    {
        CheckPattern();
        Move();
        if (canTurn)
        {
            Turn();
        }
        TimerCountDown();

    }

    void CheckPattern()
    {
        MyDebugLib.MessageLog(currentPattern);
        if (((targetObject.transform.position - gameObject.transform.position).magnitude <= attackDistance ||
            attackActionTimer > 0.0f) && currentPattern != MovePattern.KnockBack)
        {
            if (attackActionTimer <= 0.0f)
            {
                isAttacking = false;
            }
            if (attackActionTimer <= 0.0f && attackCoolDownTimer > 0.0f)
            {
                currentPattern = MovePattern.Idle;
                canTurn = true;   
            }
            else
            {
                currentPattern = MovePattern.Attack;
                canTurn = false;
                isAcceleration = false;
                Attack();
            }
        }
        else
        {
            canTurn = true;
            isAcceleration = true;
            currentPattern = MovePattern.Move;
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
            attackActionTimer = attackActionTime;
            attackCoolDownTimer = attackCoolDownTime;
            isAttacking = true;
        }
    }

    protected override void Damage()
    {

    }

    protected override void KnockBack()
    {

    }

    protected override void Death()
    {

    }


}
