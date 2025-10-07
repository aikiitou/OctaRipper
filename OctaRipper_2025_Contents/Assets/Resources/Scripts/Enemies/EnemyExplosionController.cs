using UnityEngine;

public class EnemyExplosionController : MonoBehaviour
{
    GameObject gParentObject; // 親オブジェクト格納用
    float fDamage; // 与ダメージ
    float fForce; // 吹っ飛ばし力
    float fFriezeTime; // 硬直時間

    float fAliveTime = 0.5f; // 生存時間
    float fAliveTimer = 0.0f; // 生存時間カウント

    public void SetUp(GameObject _obj, float _force, float _damage, float _friezeTime)
    {
        transform.parent = null;
        gameObject.SetActive(true);
        gParentObject = _obj;
        fDamage = _damage;
        fForce = _force;
        fFriezeTime = _friezeTime;
        fAliveTimer = fAliveTime;
    }

    void Update()
    {
        if (fAliveTimer > 0.0f)
        {
            fAliveTimer -= Time.deltaTime;
            if (fAliveTimer <= 0.0f)
            {
                Release();
            }
        }
    }
    public void Release()
    {
        transform.position = gParentObject.transform.position;
        transform.parent = gParentObject.transform;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyControllerBase>())
        {
            Vector3 distance = other.transform.position - gameObject.transform.position;
            other.GetComponent<EnemyControllerBase>().Damage(-fDamage, distance * fForce, fFriezeTime);
        }
    }
}
