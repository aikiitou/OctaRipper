using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;  //エフェクトを再生するパーティクルシステム
    [SerializeField] int nParticleEmit;
    public void HitParticle()
    {
        hitEffect.Emit(nParticleEmit);
    }
    
    /*[SerializeField] HitEffect hitEffect;     呼び出し例
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            hitEffect.HitParticle();
        }
    }*/
}