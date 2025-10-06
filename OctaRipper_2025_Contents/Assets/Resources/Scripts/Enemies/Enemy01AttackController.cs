using UnityEngine;

public class Enemy01AttackController : MonoBehaviour
{
    float fDamage; // ó^É_ÉÅÅ[ÉW

    public void SetUp(float _damage)
    {
        fDamage = _damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Damage(-fDamage, Vector3.zero);
            gameObject.SetActive(false);
        }
    }
}
