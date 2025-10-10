using UnityEngine;

public class Enemy01AttackController : MonoBehaviour
{
    float fDamage; // ó^É_ÉÅÅ[ÉW
    float fForce; // êÅÇ¡îÚÇŒÇµóÕ
    float fFriezeFrame; // çdíºéûä‘


    public void SetUp(float _damage, float _force, float _friezeTime)
    {
        fDamage = _damage;
        fForce = _force;
        fFriezeFrame = _friezeTime / (1.0f / (float)Application.targetFrameRate);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 distance = other.transform.position - gameObject.transform.position;
            other.GetComponent<PlayerController>().Damage(-fDamage, distance.normalized * fForce, (int)fFriezeFrame);
            gameObject.SetActive(false);
        }
    }
}
