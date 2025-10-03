using UnityEngine;

public class Charge1 : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.linearVelocity = new Vector3(transform.forward * 4f, 0f, 0f);
    }
}
