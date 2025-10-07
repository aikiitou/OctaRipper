using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;  //エフェクトを再生するパーティクルシステム
    void Start()
    {
        
    }

    void Update()
    {
        HitParticle();
    }
    void HitParticle()
    {
        hitEffect.Emit(1);
    }
}
