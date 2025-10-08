using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect1;  //エフェクトを再生するパーティクルシステム
    [SerializeField] ParticleSystem hitEffect2;
    public void HitParticle()
    {
        hitEffect1.Emit(10);
        hitEffect2.Emit(4);
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