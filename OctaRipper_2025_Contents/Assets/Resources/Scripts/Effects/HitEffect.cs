using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;  //エフェクトを再生するパーティクルシステム
    private bool hitEffectActive = false;       //ダメージが起きたか判別する
    void Update()
    {
        HitParticle();
        HitEffectActive();
    }
    void HitParticle()
    {
        if (hitEffectActive)
        {
            hitEffect.Emit(1);
            hitEffectActive = false;
        }
    }
    void HitEffectActive()
    {

    }
}
