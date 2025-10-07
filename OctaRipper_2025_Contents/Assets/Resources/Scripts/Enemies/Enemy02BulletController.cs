using UnityEngine;

public class Enemy02BulletController : MonoBehaviour
{
    GameObject gParentObject; // 親オブジェクト格納用
    float fDamage; // 与ダメージ
    float fForce; // 吹っ飛ばし力
    float fFriezeFrame; // 硬直時間
    Rigidbody rRigidBody;

    public void SetUp(GameObject _obj, Vector3 _position, Vector3 _direction, float _speed, float _damage, float _force, float _friezeTime)
    {
        transform.parent = null;
        gameObject.SetActive(true);
        transform.position = _position;
        rRigidBody = GetComponent<Rigidbody>();
        gParentObject = _obj;
        fDamage = _damage;
        fForce = _force;
        fFriezeFrame = _friezeTime / (1.0f / (float)Application.targetFrameRate);
        rRigidBody.linearVelocity = _direction * _speed;
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
        if (other.tag == "Player")
        {
            Vector3 distance = other.transform.position - gameObject.transform.position;
            other.GetComponent<PlayerController>().Damage(-fDamage, distance.normalized * fForce, (int)fFriezeFrame);
        }
        if (other.gameObject != gParentObject)
        {
            Release();
        }
    }
}
