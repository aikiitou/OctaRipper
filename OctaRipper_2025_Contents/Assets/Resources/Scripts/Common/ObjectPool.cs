using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    private struct GenerateInformation
    {
        public GameObject gObject;
        public int nNumber;
    }

    [SerializeField]
    private GenerateInformation sGenerateInformation;

    private Stack<GameObject> gObjectPools = new Stack<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneratePool();
    }

    private void GeneratePool()
    {
        if(sGenerateInformation.gObject == null)
        {
            return;
        }

        for(int i = 0; i < sGenerateInformation.nNumber; ++i)
        {
            GameObject obj = Instantiate(sGenerateInformation.gObject);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            gObjectPools.Push(obj);
        }
    }

    public GameObject GetPoolObject()
    {
        if (gObjectPools.Count == 0)
        {
            MyDebugLib.MessageLog("ListisEmpty");
            return null;
        }

        MyDebugLib.MessageLog("GetSuccess");

        GameObject nextObject = gObjectPools.Pop();
        nextObject.SetActive(true);
        return nextObject;
    }

    public void ReturnPoolObject(GameObject return_object)
    {
        return_object.transform.parent = this.transform;
        return_object.SetActive(false);
        gObjectPools.Push(return_object);

        MyDebugLib.MessageLog("ReturnSuccess");
    }
}
