using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackColliderController : MonoBehaviour
{
    const float HIT_STOP_TIME = 0.1f;

    List<GameObject> gDamagedObjects = new List<GameObject>();
    private Transform tPlayer;
    private float fDamage = 0f;
    private float fFreezeTime = 0f;
    private float fKnockBackPower = 0f;
    private bool bIsHitStop = false;

    private void Awake()
    {
        tPlayer = transform.parent.parent;
    }
    private void OnTriggerStay(Collider _other)
    {
        if(_other.TryGetComponent<EnemyControllerBase>(out EnemyControllerBase enemy))
        {
            if (gDamagedObjects.Contains(_other.gameObject) == false)
            {
                gDamagedObjects.Add(_other.gameObject);
                //yŽ²‚ð‚È‚­‚·
                Vector3 playerPos = new Vector3(tPlayer.position.x, 0, tPlayer.position.z);
                Vector3 enemyPos = new Vector3(_other.transform.position.x, 0, _other.transform.position.z);
                Vector3 forceVec = (enemyPos - playerPos).normalized;
                float distance = (enemyPos - playerPos).magnitude;
                forceVec *= Mathf.Lerp(1, fKnockBackPower, distance);
                enemy.Damage(-fDamage, forceVec, fFreezeTime);
                if(bIsHitStop == false)
                {
                    HitStop();
                }
            }
        }
        if(_other.TryGetComponent<FireWallController>(out FireWallController fireWall))
        {
            fireWall.TakeDamage(-fDamage);
            if (bIsHitStop == false)
            {
                HitStop();
            }
        }
    }
    private IEnumerator HitStop()
    {
        bIsHitStop = true;
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(HIT_STOP_TIME);
        Time.timeScale = 1.0f;
    }
    public void SetAttackInfo(float _damage,float _freezeTime,float _knockBackPower)
    {
        this.fDamage = _damage;
        this.fFreezeTime = _freezeTime; 
        this.fKnockBackPower = _knockBackPower;
    }
    public void ResetAttackInfo()
    {
        gDamagedObjects.Clear();
        fDamage = 0f;
        fFreezeTime = 0f;
        fKnockBackPower = 0f;
        bIsHitStop = false;
    }
}
