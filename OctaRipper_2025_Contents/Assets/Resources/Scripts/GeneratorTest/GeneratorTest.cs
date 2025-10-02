using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class GeneratorTest : MonoBehaviour
{
    private Dictionary<string, int> tagToPoolIndex = new Dictionary<string, int>()
    {
        {"Enemy01",0 },
        {"Enemy02",1 },
        {"Enemy03",2 },
    };

    [SerializeField]
    private List<ObjectPool> objectPools = new List<ObjectPool>();

    private int nCreateCount = 0;
    private float fTimer = 0.0f;
    private int nTimeCount = 0;
    void Start()
    {
        for(int i = 0; i < objectPools.Count; i++)
        {
            SetObject(i);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        fTimer += Time.deltaTime;
        if(fTimer > 1.0f)
        {
            nTimeCount++;
            if (nTimeCount % 2 == 0)
            {
                SetObject(0);
            }
            if (nTimeCount % 3 == 0)
            {
                SetObject(1);
            }
            if (nTimeCount % 4 == 0)
            {
                SetObject(2);
            }
            fTimer = 0.0f;
        }
    }

    private void SetObject(int _num)
    {
        GameObject obj = objectPools[_num].GetPoolObject();

        if(obj == null)
        {
            return;
        }

        obj.transform.parent = this.transform;
        obj.transform.position = new Vector3(-10 + nCreateCount % 20, 5 - nCreateCount / 10, 0.0f);
        nCreateCount++;

    }

    public void Release(GameObject release_object)
    {
        MyDebugLib.MessageLog("Release");

        if(tagToPoolIndex.TryGetValue(release_object.tag, out int index))
        {
            MyDebugLib.MessageLog(release_object.tag);
            objectPools[index].ReturnPoolObject(release_object);
        }
        else
        {
            MyDebugLib.MessageLog(release_object.tag);
            MyDebugLib.MessageLog("NotTag");
        }
    }
}
