using UnityEngine;
using UnityEngine.Pool;

public abstract class EnemyControllerBase : MonoBehaviour
{
    protected EnemyLifeController enemyLifeController; // ライフポイントを管理するコンポーネント

    protected enum MovePattern
    {
        Idle,
        Move,
        Attack,
        KnockBack,
        Death,
    }
    public void InitializeEnemyData(float _lifePoint) // エネミー情報の初期化
    {
        enemyLifeController = gameObject.GetComponent<EnemyLifeController>(); // コンポーネントの代入
        enemyLifeController.SetLifePoint(_lifePoint); // ライフポイントのセット
        enemyLifeController.SetInvincible(false); // 無敵の解除
    }

    public void ReleaseObject(GameObject _obj)
    {
        enemyLifeController.SetInvincible(true); // 無敵の有効
        //transform.parent.GetComponent<>().
    }

    protected abstract void Move(); // 移動

    protected abstract void Turn(); // 回転

    protected abstract void Attack(); // 攻撃

    protected abstract void Damage(); // 被ダメージ
    
    protected abstract void KnockBack(); // ノックバック
    
    protected abstract void Death(); // 死亡



}
