using UnityEngine;

public class LifeController : MonoBehaviour
{
    float fLifePoint; // ライフポイント
    bool bIsInvincible = true; // 無敵(ダメージを受けない)

    public float GetLifePoint => fLifePoint; // ライフポイントのゲッター

    public void SetLifePoint(float _value) // ライフポイントのセッター
    {
        fLifePoint = _value;
    }

    public void SetInvincible(bool _isInvincible) // 無敵判定のセッター
    {
        bIsInvincible = _isInvincible;
    }

    public bool ChangeLifePoint(float _value) // ライフポイントの変更
    {
        if (!bIsInvincible)
        {
            fLifePoint += _value;
            return true;
        }
        return false;
    }

}
