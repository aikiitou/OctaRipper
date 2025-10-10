using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour
{

    [SerializeField]
    GameObject[] gMissiles;

    List<GameObject> gMissilesList = new List<GameObject>();

    
    void Start()
    {
        for (int i = 0; i < gMissiles.Length; i++)
        {
            gMissilesList.Add(gMissiles[i]);
        }
    }

    public void MissileSet(GameObject _shotObject, Vector3 _pos)
    {
        gMissilesList[gMissilesList.Count - 1].gameObject.GetComponent<MissileController>().SetUp(_shotObject,gameObject, _pos);
        gMissilesList.RemoveAt(gMissilesList.Count - 1);
    }

    public void ReturnList(GameObject _missile)
    {
        gMissilesList.Add(_missile);
    }
}
