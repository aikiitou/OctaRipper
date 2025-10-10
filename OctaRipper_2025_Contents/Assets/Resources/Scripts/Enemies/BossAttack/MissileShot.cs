using UnityEngine;

public class MissileShot : MonoBehaviour
{
    [SerializeField]
    GameObject gShotPosition;

    [SerializeField]
    GameObject gMissileObjectsPool;
    void OnEnable()
    {
        gMissileObjectsPool.GetComponent<MissilePool>().MissileSet(gameObject, gShotPosition.transform.position);
    }
}
