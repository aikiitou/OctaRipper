using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> gObjectPools = new List<GameObject>();

    public void GeneratePool(GameObject create_object,int create_num)
    {
        if(create_object == null)
        {
            return;
        }

        for (int i = 0; i < create_num; i++)
        {
            GameObject obj = Instantiate(create_object);
            obj.transform.parent = this.transform;
            obj.SetActive(false);
            gObjectPools.Add(obj);
        }
    }

    public GameObject GetPoolObject(string get_object_tag)
    {
        GameObject nextObject = gObjectPools.FirstOrDefault(obj => obj.CompareTag(get_object_tag));
        if (nextObject != null)
        {
            MyDebugLib.MessageLog("GetSuccess" + nextObject);
            gObjectPools.Remove(nextObject);
            return nextObject;
        }
        else
        {
            MyDebugLib.MessageLog("ListisEmpty");
        }
        return null; 
    }

    public void ReturnPoolObject(GameObject return_object)
    {
        return_object.transform.parent = this.transform;
        gObjectPools.Add(return_object);
        MyDebugLib.MessageLog("ReturnSuccess");
    }
}
