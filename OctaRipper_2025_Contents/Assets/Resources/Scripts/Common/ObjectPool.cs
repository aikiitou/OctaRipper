using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Stack<GameObject> gObjectPools = new Stack<GameObject>();

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
            gObjectPools.Push(obj);
        }
    }

    public GameObject GetPoolObject(string get_object_tag)
    {
        if(gObjectPools.Any(obj => obj.CompareTag(get_object_tag)))
        {
            MyDebugLib.MessageLog("GetSuccess");
            GameObject nextObject = gObjectPools.Pop();
            return nextObject;
        }
        else
        {
            MyDebugLib.MessageLog("ListisEmpty");
            return null;
        }

    }

    public void ReturnPoolObject(GameObject return_object)
    {
        return_object.transform.parent = this.transform;
        gObjectPools.Push(return_object);
        MyDebugLib.MessageLog("ReturnSuccess");
    }
}
