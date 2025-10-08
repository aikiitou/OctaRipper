using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Charge1 : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float fSpeed;
    [SerializeField] float fDamage;
    [SerializeField] float fFreezeTime;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * fSpeed, ForceMode.VelocityChange);
    }
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent<EnemyControllerBase>(out EnemyControllerBase enemy))
        {
            enemy.Damage(-fDamage, transform.forward, fFreezeTime);
        }
    }
}
    