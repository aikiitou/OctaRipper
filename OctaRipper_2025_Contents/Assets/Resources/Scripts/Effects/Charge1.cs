using UnityEngine;

public class Charge1 : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float fSpeed;
    [SerializeField] float fDamage;
    [SerializeField] float fFreezeTime;
    [SerializeField] float fKnockBackPower;
    [SerializeField] float fDestroyTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * fSpeed, ForceMode.VelocityChange);
        Destroy(gameObject, fDestroyTime);
    }
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent<EnemyControllerBase>(out EnemyControllerBase enemy))
        {
            enemy.Damage(-fDamage, transform.forward * fKnockBackPower, fFreezeTime);
        }
        if (_other.TryGetComponent<FireWallController>(out FireWallController fireWall))
        {
            fireWall.TakeDamage(-fDamage);
            Destroy(gameObject);
        }
    }
}
    