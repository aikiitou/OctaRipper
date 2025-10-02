using UnityEngine;

public class EnemyLifeController : MonoBehaviour
{
    float lifePoint; // ライフポイント
    bool isInvincible = true; // 無敵(ダメージを受けない)

    public float LifePoint => lifePoint; // ライフポイントのゲッター

    public void SetLifePoint(float _value) // ライフポイントのセッター
    {
        lifePoint = _value;
    }

    public void SetInvincible(bool _isInvincible) // 無敵判定のセッター
    {
        isInvincible = _isInvincible;
    }

    public void ChangeLifePoint(float _value) // ライフポイントの変更
    {
        if (!isInvincible)
        {
            lifePoint += _value;
        }
    }

}
