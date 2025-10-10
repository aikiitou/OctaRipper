using System.Collections;
using UnityEngine;

public class BossController : EnemyControllerBase
{
    [SerializeField, Header("初期ライフポイント")]
    float fInitLifePoint = 100.0f;

    [SerializeField, Header("与ダメージ")]
    float fAttackDamage = 10.0f;

    [SerializeField, Header("吹っ飛ばし")]
    float fAttackForce = 1.0f;

    [SerializeField, Header("与硬直時間")]
    float fAttackFriezeTime = 0.1f;

    [SerializeField, Header("攻撃の当たり判定")]
    GameObject gDamageTrigger; // 攻撃の当たり判定オブジェクト

    [SerializeField, Header("見た目回転スピード")]
    float fRotationSpeed = 10.0f;

    [SerializeField, Header("ヒットエフェクトオブジェクト")]
    GameObject gHitEffect; // 攻撃の当たり判定オブジェクト


    [SerializeField, Header("最遅モーションスピード")]
    float fSlowestSpeed;

    [SerializeField, Header("初動無敵時間")]
    float fFirstInvincibleTime;

    [SerializeField, Header("行動間秒数")]
    float fIdleTime;

    [SerializeField, Header("降り下ろし時間")]
    float fFallFistTime;

    [SerializeField, Header("薙ぎ払い時間")]
    float fSweepTime;

    [SerializeField, Header("暴走時間")]
    float fWildTime;

    [SerializeField, Header("暴走クールダウン時間")]
    float fWildCoolDownTime;

    [SerializeField, Header("死亡時爆発与ダメージ")]
    float fExplosionDamage = 0.0f;

    [SerializeField, Header("死亡時爆発与硬直時間")]
    float fExplosionFriezeTime = 0.0f;

    [SerializeField, Header("死亡時爆発吹っ飛ばし")]
    float fExplosionForce = 0.0f;

    [SerializeField, Header("爆発オブジェクト")]
    GameObject gExplosion; // 攻撃の当たり判定オブジェクト

    bool bIsDown = false; // 暴走によるダウンをしているかどうか
    bool bCanTurn = false; // 回転可能かどうか
    float fDeadDelayTime = 5.0f; // 死亡遅延時間
    float fFriezeTimer; // 硬直時間
    float fActionTimer; // 行動時間
    int nActionNum; // 行動ナンバー
    GameObject gTargetObject; // 対象のオブジェクト
    GameObject gClearObject; // ゲームクリアのオブジェクト
    Rigidbody rRigidbody; // Rigidbody
    Animator aAnimator; // Animator


    void Start()
    {
        gDamageTrigger.SetActive(false);
        aAnimator = GetComponent<Animator>();
        gTargetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        gClearObject = GameObject.FindGameObjectWithTag("SceneChanger"); // 対象を代入
        rRigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        InitializeEnemyData(fInitLifePoint); // 初期ライフポイントのセット
        cLifeController.SetInvincible(true);
        fActionTimer = fFirstInvincibleTime;
        gDamageTrigger.GetComponent<Enemy01AttackController>().SetUp(fAttackDamage, fAttackForce, fAttackFriezeTime);
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
            StartCoroutine(DeadDelay()); // 死亡
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(-100.0f, Vector3.zero, 0.0f);
        }
    }

    void ChangePattern()
    {
        if (bIsDown)
        {
            aAnimator.SetFloat("MotionMultiple", fSlowestSpeed + (cLifeController.GetLifePoint / fInitLifePoint * (1.0f - fSlowestSpeed)));
        }
        else
        {
            aAnimator.SetFloat("MotionMultiple", 1.0f);
        }
    }

    void TimerCountDown() // 各タイマーのカウントダウン
    {
        if (fActionTimer > 0.0f)
        {
            if (bIsDown)
            {
                fActionTimer -= Time.deltaTime * (fSlowestSpeed + (cLifeController.GetLifePoint / fInitLifePoint * (1.0f - fSlowestSpeed)));
            }
            else
            {
                fActionTimer -= Time.deltaTime;
            }
        }
        else
        {
            switch (nActionNum)
            {
                case 0:
                    cLifeController.SetInvincible(false);
                    fActionTimer += fIdleTime;
                    aAnimator.SetTrigger("Start");
                    bCanTurn = true;
                    break;
                case 1:
                    fActionTimer += fFallFistTime;
                    aAnimator.SetBool("Attack", true);
                    bCanTurn = false;
                    break;
                case 2:
                    fActionTimer += fIdleTime;
                    aAnimator.SetBool("Attack", false);
                    bCanTurn = true;
                    break;
                case 3:
                    fActionTimer += fSweepTime;
                    aAnimator.SetBool("Attack", true);
                    bCanTurn = false;
                    break;
                case 4:
                    fActionTimer += fIdleTime;
                    aAnimator.SetBool("Attack", false);
                    bCanTurn = true;
                    break;
                case 5:
                    fActionTimer += fFallFistTime;
                    aAnimator.SetBool("Attack", true);
                    bCanTurn = false;
                    break;
                case 6:
                    fActionTimer += fIdleTime;
                    aAnimator.SetBool("Attack", false);
                    bCanTurn = true;
                    break;
                case 7:
                    fActionTimer += fWildTime;
                    aAnimator.SetBool("Attack", true);
                    break;
                case 8:
                    fActionTimer += fWildCoolDownTime;
                    bCanTurn = false;
                    bIsDown = true;
                    break;
                case 9:
                    fActionTimer += fIdleTime;
                    aAnimator.SetBool("Attack", false);
                    nActionNum = 0;
                    bCanTurn = true;
                    bIsDown = false;
                    break;
            }
            ++nActionNum;
        }
    }
    protected override void Move() // 移動
    {
    }

    protected override void Turn() // 回転
    {
        Vector3 horizonDistance = gTargetObject.transform.position - gameObject.transform.position;
        horizonDistance = new Vector3(horizonDistance.x, 0.0f, horizonDistance.z);
        if (Mathf.Abs(horizonDistance.x) <= 0.01f)
        {
            horizonDistance = new Vector3(0.01f, horizonDistance.y, horizonDistance.z);
        }
        if (Mathf.Abs(horizonDistance.z) <= 0.01f)
        {
            horizonDistance = new Vector3(horizonDistance.x, horizonDistance.y, 0.01f);
        }
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.FromToRotation(Vector3.forward, horizonDistance.normalized),
            fRotationSpeed * Time.deltaTime
            ); // 方向転換
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
    }

    protected override void Attack() // 攻撃
    {
    }

    public override void Damage(float _damage, Vector3 _impact, float _friezeTime) // ダメージ処理
    {
        gHitEffect.GetComponent<HitEffect>().HitParticle();
        cLifeController.ChangeLifePoint(_damage); // ダメージを与える
    }

    protected override void KnockBack(Vector3 _force, float _friezeTime) // ノックバック・硬直
    {
    }

    protected override void Death() // 死亡(現在は仮ログ)
    {
        gExplosion.GetComponent<EnemyExplosionController>().SetUp(gameObject, fExplosionForce, fExplosionDamage, fExplosionFriezeTime);
        MyDebugLib.MessageLog("Dead");
    }

    IEnumerator DeadDelay()
    {
        aAnimator.SetTrigger("Dead");
        fFriezeTimer = fDeadDelayTime;
        yield return new WaitForSeconds(fDeadDelayTime);
        Death();
    }

    IEnumerator ClearDelay()
    {
        fFriezeTimer = 1.0f;
        yield return new WaitForSeconds(fDeadDelayTime);
        //gClearObject.GetComponent<GameClearNotification>
        Death();
    }

}
