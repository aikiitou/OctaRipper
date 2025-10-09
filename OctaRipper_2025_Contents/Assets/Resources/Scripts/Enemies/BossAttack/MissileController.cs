using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField, Header("スピード")]
    float fSpeed = 10.0f;

    [SerializeField, Header("与ダメージ")]
    float fDamage = 10.0f;

    [SerializeField, Header("回転開始までの時間")]
    float fCanRotationTime = 1.0f;

    [SerializeField, Header("見た目回転スピード")]
    float fRotationSpeed = 10.0f;

    [SerializeField, Header("最大生存時間")]
    float fAliveTime = 5.0f;


    [SerializeField, Header("爆発与硬直時間")]
    float fExplosionFriezeTime = 0.3f;

    [SerializeField, Header("爆発吹っ飛ばし")]
    float fExplosionForce = 10.0f;

    [SerializeField, Header("爆発オブジェクト")]
    GameObject gExplosion; // 攻撃の当たり判定オブジェクト


    float fCanRotationTimer;
    GameObject gParentObject;
    Rigidbody rRigidBody;
    GameObject gTargetObject; // 対象のオブジェクト
    public void SetUp(GameObject _shotObject, GameObject _parent, Vector3 _pos)
    {
        fCanRotationTimer = fCanRotationTime;
        gTargetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        gParentObject = _parent;
        rRigidBody = GetComponent<Rigidbody>();
        rRigidBody.linearVelocity = -_shotObject.transform.up * fSpeed;
        transform.parent = null;
        transform.position = _pos;
        gameObject.SetActive(true);
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, rRigidBody.linearVelocity);
    }
    void Update()
    {
        if (fCanRotationTimer >= 0)
        {
            fCanRotationTimer -= Time.deltaTime;
        }
        else
        {
            rRigidBody.linearVelocity = transform.forward * fSpeed;
            Vector3 distance = gTargetObject.transform.position - gameObject.transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.FromToRotation(Vector3.forward, distance.normalized),
                fRotationSpeed * Time.deltaTime
                ); // 方向転換
        }
    }

    public void Release()
    {
        transform.position = gParentObject.transform.position;
        transform.parent = gParentObject.transform;
        rRigidBody.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 11)
        {
            gExplosion.GetComponent<EnemyExplosionController>().SetUp(gameObject, fExplosionForce, fDamage, fExplosionFriezeTime);
            MyDebugLib.MessageLog(other.gameObject.layer);
            Release();
        }
    }


}
