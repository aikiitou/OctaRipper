using System.Collections.Generic;
using UnityEngine;

public class BossBulletPool : MonoBehaviour
{

    [SerializeField]
    GameObject[] gBullets;

    List<GameObject> gBulletsList = new List<GameObject>();


    void Start()
    {
        for (int i = 0; i < gBullets.Length; i++)
        {
            gBulletsList.Add(gBullets[i]);
        }
    }

    public void BulletSet(GameObject _shotObject, Vector3 _pos)
    {
        gBulletsList[gBulletsList.Count - 1].gameObject.GetComponent<BossBulletController>().SetUp(_shotObject, gameObject, _pos);
        gBulletsList.RemoveAt(gBulletsList.Count - 1);
    }

    public void ReturnList(GameObject _bullet)
    {
        gBulletsList.Add(_bullet);
    }
}
