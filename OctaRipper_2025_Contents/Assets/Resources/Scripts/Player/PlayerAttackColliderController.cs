using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackColliderController : MonoBehaviour
{
    List<GameObject> gDamagedObjects = new List<GameObject>();
    private Transform tPlayer;
    private float fDamage = 0f;
    private float fFreezeTime = 0f;
    private float fKnockBackPower = 0f;

    private void Awake()
    {
        tPlayer = transform.root;
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
                forceVec *= fKnockBackPower;
                enemy.Damage(-fDamage, forceVec, fFreezeTime);
            }
        }
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
    }
}
