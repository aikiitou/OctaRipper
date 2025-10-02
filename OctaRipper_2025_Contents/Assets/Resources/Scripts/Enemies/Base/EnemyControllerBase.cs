using UnityEngine;
using UnityEngine.Pool;

public abstract class EnemyControllerBase : MonoBehaviour
{
    EnemyLifeController enemyLifeController;
    public EnemyControllerBase()
    {
        enemyLifeController = gameObject.AddComponent<EnemyLifeController>();
    }

    public void InitializeEnemyData(float _lifePoint)
    {
        enemyLifeController.SetLifePoint(_lifePoint);
        enemyLifeController.SetInvincible(false);
    }

    public abstract void Move();
    
    public abstract void Attack();
    
    public abstract void KnockBack();
    
    public abstract void Spawn();
    
    public abstract void Death();
    


}
