using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    [SerializeField, Header("スピード")]
    float fSpeed = 20.0f;

    [SerializeField, Header("与ダメージ")]
    float fDamage = 10.0f;

    [SerializeField, Header("最大生存時間")]
    float fAliveTime = 1.0f;


    [SerializeField, Header("爆発与硬直時間")]
    float fExplosionFriezeTime = 0.1f;

    [SerializeField, Header("爆発吹っ飛ばし")]
    float fExplosionForce = 5.0f;

    [SerializeField, Header("爆発オブジェクト")]
    GameObject gExplosion; // 攻撃の当たり判定オブジェクト


    float fAliveTimer;
    GameObject gParentObject;
    Rigidbody rRigidBody;
    GameObject gTargetObject; // 対象のオブジェクト
    public void SetUp(GameObject _shotObject, GameObject _parent, Vector3 _pos)
    {
        fAliveTimer = fAliveTime;
        gTargetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        gParentObject = _parent;
        rRigidBody = GetComponent<Rigidbody>();
        rRigidBody.linearVelocity = _shotObject.transform.up * fSpeed;
        transform.parent = null;
        transform.position = _pos;
        gameObject.SetActive(true);
        Vector3 distance = gTargetObject.transform.position - gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.FromToRotation(Vector3.forward, distance.normalized),
            1.0f
            ); // 方向転換
    }
    void Update()
    {
        rRigidBody.linearVelocity = transform.forward * fSpeed;
        fAliveTimer -= Time.deltaTime;
        if (fAliveTimer < 0.0f)
        {
            Release();
        }

    }

    public void Release()
    {
        transform.position = gParentObject.transform.position;
        gParentObject.GetComponent<BossBulletPool>().ReturnList(gameObject);
        transform.parent = gParentObject.transform;
        rRigidBody.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 11)
        {
            gExplosion.GetComponent<EnemyExplosionController>().SetUp(gameObject, fExplosionForce, fDamage, fExplosionFriezeTime, true);
            Release();
        }
    }

}
