using UnityEngine;

public class BossBulletShot : MonoBehaviour
{
    [SerializeField]
    GameObject gShotPosition;

    [SerializeField]
    GameObject gBulletObjectsPool;

    [SerializeField]
    float fShotRate = 8.0f;

    float fShotTimer;

    void Update()
    {
        if (fShotTimer <= 0.0f)
        {
            fShotTimer = 1.0f;
            gBulletObjectsPool.GetComponent<BossBulletPool>().BulletSet(gameObject, gShotPosition.transform.position);
        }
        fShotTimer -= Time.deltaTime * fShotRate;
    }
}
