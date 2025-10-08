using UnityEngine;

public class Enemy03AttackController : MonoBehaviour
{
    float fDamage; // 与ダメージ
    Vector3 vImpact; // 吹っ飛ばし力・方向
    float fFriezeFrame; // 硬直時間


    public void SetUp(float _damage, Vector3 _impact, float _friezeTime)
    {
        fDamage = _damage;
        vImpact = _impact;
        fFriezeFrame = _friezeTime / (1.0f / (float)Application.targetFrameRate);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Damage(-fDamage, vImpact, (int)fFriezeFrame);
            gameObject.SetActive(false);
        }
    }
}
