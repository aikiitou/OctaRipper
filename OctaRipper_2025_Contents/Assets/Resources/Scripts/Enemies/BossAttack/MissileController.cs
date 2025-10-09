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

    GameObject gParentObject;
    Rigidbody rRigidBody;
    GameObject gTargetObject; // 対象のオブジェクト
    public void SetUp(GameObject _shotObject, GameObject _parent, Vector3 _pos)
    {
        gTargetObject = GameObject.FindGameObjectWithTag("Player"); // 対象を代入
        gParentObject = _parent;
        rRigidBody = GetComponent<Rigidbody>();
        rRigidBody.linearVelocity = _shotObject.transform.up * fSpeed;
        transform.rotation = Quaternion.FromToRotation(transform.position, _shotObject.transform.position);
        transform.parent = null;
        transform.position = _pos;
        gameObject.SetActive(true);
    }
    void Update()
    {
        rRigidBody.linearVelocity = transform.forward * fSpeed;
        Vector3 distance = gTargetObject.transform.position - gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.FromToRotation(Vector3.forward, distance.normalized),
            fRotationSpeed * Time.deltaTime
            ); // 方向転換
    }

    public void Release()
    {
        transform.position = gParentObject.transform.position;
        transform.parent = gParentObject.transform;
        rRigidBody.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }



}
