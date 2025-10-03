using UnityEngine;
using UnityEngine.Pool;

public abstract class EnemyControllerBase : MonoBehaviour
{
    protected EnemyLifeController cEnemyLifeController; // ライフポイントを管理するコンポーネント

    public void InitializeEnemyData(float _lifePoint) // エネミー情報の初期化
    {
        cEnemyLifeController = gameObject.GetComponent<EnemyLifeController>(); // コンポーネントの代入
        cEnemyLifeController.SetLifePoint(_lifePoint); // ライフポイントのセット
        cEnemyLifeController.SetInvincible(false); // 無敵の解除
    }

    public void ReleaseObject(GameObject _obj)
    {
        cEnemyLifeController.SetInvincible(true); // 無敵の有効
        //transform.parent.GetComponent<>().
    }

    protected abstract void Move(); // 移動

    protected abstract void Turn(); // 回転

    protected abstract void Attack(); // 攻撃

    public abstract void Damage(float _damage, Vector3 _force, float _friezeTime); // 被ダメージ
    
    protected abstract void KnockBack(Vector3 _force, float _friezeTime); // ノックバック
    
    protected abstract void Death(); // 死亡



}
