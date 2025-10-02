using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [System.Serializable]
    private struct GenerateObject
    {
        public GameObject generateObject;
        public int generateNum;
    }

    [SerializeField]
    private GenerateObject sGenerateObject;

    private List<GameObject> gObjectPools = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        if(sGenerateObject.generateObject == null)
        {
            return;
        }

        for(int i = 0; i < sGenerateObject.generateNum; ++i)
        {
            GameObject obj = Instantiate(sGenerateObject.generateObject);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            gObjectPools.Add(obj);
        }

    }
}
