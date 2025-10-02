using UnityEngine;

public class EnemyController01 : EnemyControllerBase
{
    [SerializeField,Header("初期ライフポイント")]
    float initLifePoint = 100.0f;

    [SerializeField, Header("最大移動速度")]
    float maxSpeed = 5.0f;

    [SerializeField, Header("移動加速度")]
    float acceleration = 5.0f;

    [SerializeField, Header("移動減速度")]
    float brake = 5.0f;

    MovePattern currentPattern;
    Vector3 moveForce;
    GameObject targetObject;
    Rigidbody rb;
    bool isAcceleration;


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
        
    }

    protected override void Move()
    {

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
