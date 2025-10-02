using UnityEngine;

public class EnemyController01 : EnemyControllerBase
{
    [SerializeField,Header("初期ライフポイント")]
    float initLifePoint = 100.0f;

    [SerializeField, Header("最大移動速度")]
    float maxSpeed = 2.0f;

    [SerializeField, Header("移動加速度")]
    float acceleration = 5.0f;

    [SerializeField, Header("移動減速度")]
    float brake = 10.0f;

    [SerializeField, Header("攻撃実行距離")]
    float attackDistance = 1.0f;

    MovePattern currentPattern;
    Vector3 moveForce;
    GameObject targetObject;
    Rigidbody rb;
    bool isAcceleration = true;


    void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        InitializeEnemyData(initLifePoint); // 初期ライフポイントのセット
    }

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (isAcceleration)
        {
            Vector3 moveAddForce = targetObject.transform.position - gameObject.transform.position; // 対象と自分の距離算出
            moveAddForce = new Vector3(moveAddForce.x, 0.0f, moveAddForce.z); // y成分を除く
            moveAddForce = moveAddForce.normalized * acceleration * Time.deltaTime; // 加速量算出
            moveForce += moveAddForce;
            if (moveForce.magnitude >= maxSpeed)
            {
                moveForce = moveForce.normalized * maxSpeed;
            }
            MyDebugLib.MessageLog(moveForce.magnitude);
        }
        else
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

    protected override void Attack()
    {

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
