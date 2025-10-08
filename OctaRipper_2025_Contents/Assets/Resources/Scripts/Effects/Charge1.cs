using UnityEngine;

public class Charge1 : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float fSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * -fSpeed, ForceMode.VelocityChange);
    }
}
    