using UnityEngine;

public class EnemyController02 : EnemyControllerBase
{
    [SerializeField, Header("初期ライフポイント")]
    float fInitLifePoint = 30.0f;

    [SerializeField, Header("最大移動速度")]
    float fMaxSpeed = 2.0f;

    [SerializeField, Header("移動加速度")]
    float fAcceleration = 20.0f;

    [SerializeField, Header("移動減速度")]
    float fBrake = 20.0f;

    [SerializeField, Header("攻撃に必要な距離")]
    float fAttackDistance = 20.0f;

    [SerializeField, Header("移動クールダウン")]
    float fMoveCoolDownTime = 3.0f;

    [SerializeField, Header("弾速")]
    float fBulletSpeed = 20.0f;

    [SerializeField, Header("与ダメージ")]
    float fAttackDamage = 10.0f;

    [SerializeField, Header("吹っ飛ばし")]
    float fAttackForce = 1.0f;

    [SerializeField, Header("与硬直時間")]
    float fAttackFriezeTime = 0.1f;

    [SerializeField, Header("攻撃クールダウン")]
    float fAttackCoolDownTime = 2.0f;

    [SerializeField, Header("見た目回転スピード")]
    float fRotationSpeed = 10.0f;

    [SerializeField, Header("弾")]
    GameObject gBulletObject; // 弾丸

    [SerializeField, Header("射撃位置")]
    GameObject gBulletShotPosition; // 射撃位置の空オブジェクト

    [SerializeField, Header("死亡時爆発与ダメージ")]
    float fExplosionDamage = 10.0f;

    [SerializeField, Header("死亡時爆発与硬直時間")]
    float fExplosionFriezeTime = 10.0f;

    [SerializeField, Header("死亡時爆発吹っ飛ばし")]
    float fExplosionForce = 10.0f;

    bool bIsAcceleration = true; // 現在加速しているかどうか
    bool bIsAttacking = false; // 攻撃しているかどうか
    bool bCanTurn = true; // 回転可能かどうか
    bool bCanAction = true; // 行動可能かどうか
    float fAttackCoolDownTimer;
    float fMoveCoolDownTimer;
    float fFriezeTimer; // 硬直時間
    Vector3 vMoveForce; // 移動量
    GameObject gTargetObject; // 対象のオブジェクト
    Rigidbody rRigidbody; // Rigidbody
    Animator aAnimator; // Animator


    void Start()
    {
        fAttackCoolDownTimer = fAttackCoolDownTime;
        gBulletObject.SetActive(false);
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
        if (cLifeController.GetLifePoint <= 0.0f)
        {
            Death(); // 死亡
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(-10.0f, (gameObject.transform.position - gTargetObject.transform.position).normalized * 5.0f, 1.0f);
        }
    }

    void ChangePattern()
    {
        if (fFriezeTimer <= 0.0f) // 硬直状態のタイマーが終わっていたら
        {
            bCanAction = true; // 行動を許可する
        }
        else
        {
            return; // でなければ攻撃を停止させ、returnする。
        }
        if (fMoveCoolDownTimer <= 0.0f) // 攻撃中のタイマーが終わっていれば、攻撃判定を終了させ、回転を許可する。
        {
            bCanTurn = true;
            bIsAcceleration = true;
        }
        if (fAttackCoolDownTimer <= 0.0f && rRigidbody.linearVelocity.magnitude <= 0.0f && bCanAction) // 攻撃距離内に対象がいるまたは攻撃実行中、かつ行動可能。
        {
            bIsAttacking = false;
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Player") // 正面にプレイヤーがいる際に攻撃実行。
                {
                    bCanTurn = false;
                    Attack();
                }
            }
        }
        if (!bIsAttacking) // 攻撃中でなければ
        {
            bCanTurn = true;
        }
    }

    void TimerCountDown() // 各タイマーのカウントダウン
    {
        if (fAttackCoolDownTimer > 0.0f &&
            Vector3.Distance(gTargetObject.transform.position, transform.position) > fAttackDistance)
        {
            fAttackCoolDownTimer -= Time.deltaTime;
        }
        if (fMoveCoolDownTimer > 0.0f &&
            Vector3.Distance(gTargetObject.transform.position, transform.position) <= fAttackDistance)
        {
            fMoveCoolDownTimer -= Time.deltaTime;
        }
        if (fFriezeTimer > 0.0f)
        {
            fFriezeTimer -= Time.deltaTime;
        }
    }
    protected override void Move() // 移動
    {
        if(bIsAcceleration)
        {
            Vector3 moveAddForce = gTargetObject.transform.position - gameObject.transform.position; // 対象と自分の距離算出
            moveAddForce = new Vector3(moveAddForce.x, 0.0f, moveAddForce.z); // y成分を除く
            moveAddForce = moveAddForce.normalized * fAcceleration; // 加速量算出
            vMoveForce -= moveAddForce; // 加速度を移動量に加える
            fMoveCoolDownTimer = fMoveCoolDownTime;
            bIsAcceleration = false;
        }
        vMoveForce -= vMoveForce.normalized * fBrake * Time.deltaTime; // 摩擦
        if (vMoveForce.magnitude >= fBrake * Time.deltaTime)
        {
            vMoveForce -= vMoveForce.normalized * fBrake * Time.deltaTime;
        }
        else
        {
            vMoveForce = Vector3.zero;
        }
        rRigidbody.linearVelocity = new Vector3(vMoveForce.x, rRigidbody.linearVelocity.y, vMoveForce.z); // 反映
    }

    protected override void Turn() // 回転
    {
        Vector3 horizonDistance = gTargetObject.transform.position - gameObject.transform.position;
        horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.FromToRotation(Vector3.forward, horizonDistance.normalized),
            fRotationSpeed * Time.deltaTime
            ); // 方向転換
    }

    protected override void Attack() // 攻撃
    {
        if (!bIsAttacking) // 初動処理
        {
            Vector3 horizonDistance = gTargetObject.transform.position - gameObject.transform.position;
            horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
            aAnimator.SetTrigger("IsAttacking");
            gBulletObject.GetComponent<Enemy02BulletController>().SetUp(
                gameObject,
                gBulletShotPosition.transform.position,
                horizonDistance.normalized,
                fBulletSpeed,
                fAttackDamage,
                fAttackForce,
                fFriezeTimer
                );
            fAttackCoolDownTimer = fAttackCoolDownTime;
            bIsAttacking = true;
        }
    }

    public override void Damage(float _damage, Vector3 _impact, float _friezeTime) // ダメージ処理
    {
        cLifeController.ChangeLifePoint(_damage); // ダメージを与える
        if (_friezeTime > 0) // 硬直時間が存在するのであれば、ノックバックと硬直を発生させる。
        {
            KnockBack(_impact, _friezeTime);
        }
    }

    protected override void KnockBack(Vector3 _force, float _friezeTime) // ノックバック・硬直
    {
        aAnimator.SetTrigger("Damaged"); // 硬直モーション起動
        bIsAcceleration = false; // 加速停止
        bCanAction = false; // 行動停止
        vMoveForce = _force; // 移動力を吹っ飛ばされる力に上書き
        rRigidbody.linearVelocity = vMoveForce; // 反映
        fFriezeTimer = _friezeTime; // 硬直時間の設定
    }

    protected override void Death() // 死亡(現在は仮ログ)
    {
        MyDebugLib.MessageLog("Dead");
        gameObject.SetActive(false);
    }


}
