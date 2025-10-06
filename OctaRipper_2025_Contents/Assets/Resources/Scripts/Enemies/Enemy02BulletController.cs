using UnityEngine;

public class Enemy02BulletController : MonoBehaviour
{
    GameObject gParentObject; // 親オブジェクト格納用
    float fDamage; // 与ダメージ
    Rigidbody rRigidBody;

    public void SetUp(GameObject _obj, Vector3 _position, Vector3 _direction, float _speed, float _damage)
    {
        transform.parent = null;
        transform.position = _position;
        rRigidBody = GetComponent<Rigidbody>();
        gParentObject = _obj;
        fDamage = _damage;
        rRigidBody.linearVelocity = _direction * _speed;
    }

    void Release()
    {
        transform.position = gParentObject.transform.position;
        transform.parent = gParentObject.transform;
        rRigidBody.linearVelocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
        }
        Release();
    }
}
