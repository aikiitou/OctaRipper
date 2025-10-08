using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    private struct CreateEnemy
    {
        public GameObject gCreateObject;
        public int nCreateNum;
    }

    
    [SerializeField]
    private List<CreateEnemy> cCreateEnemies;

    [SerializeField]
    private ObjectPool cObjectPool = null;

    private void Start()
    {
        foreach(var enemy in cCreateEnemies)
        {
            cObjectPool.GeneratePool(enemy.gCreateObject, enemy.nCreateNum);
        }
        if (gPlayerObject != null)
        {
            fCurrentPlayerPosY = gPlayerObject.transform.position.y;
        }

    }

    public GameObject RequestGetPoolObject(string _tag)
    {
        return cObjectPool.GetPoolObject(_tag);
    }

    public void RequestReturnPoolObject(GameObject game_object)
    {
        cObjectPool.ReturnPoolObject(game_object);
    }




    private enum StageNum
    {
        None,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Clear
    }

    private const float NextStageOffsetY = 2.5f;

    private Dictionary<StageNum, StageNum> cGetNextStage = new Dictionary<StageNum, StageNum>()
    {
        {StageNum.None,StageNum.Stage1 },
        {StageNum.Stage1, StageNum.Stage2 },
        {StageNum.Stage2,StageNum.Stage3},
        {StageNum.Stage3,StageNum.Stage4},
        {StageNum.Stage4, StageNum.Clear}
    };

    private StageNum cCurrentStageNum = StageNum.None;

    [SerializeField]
    private GameObject gPlayerObject = null;
    private float fCurrentPlayerPosY = 0.0f;

    private int nStageCount = 0;
    [SerializeField]
    private List<GameObject> gStageEmptys = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        MyDebugLib.MessageLog(fCurrentPlayerPosY);
        if (gPlayerObject.transform.position.y < fCurrentPlayerPosY - NextStageOffsetY)
        {
            MyDebugLib.MessageLog("StartStage");
            StartNextStage(cGetNextStage[cCurrentStageNum]);
            fCurrentPlayerPosY = gPlayerObject.transform.position.y;
        }
    }

    private void StartNextStage(StageNum next_stage)
    {
        if (next_stage == cCurrentStageNum) return;

        cCurrentStageNum = next_stage;

        gStageEmptys[nStageCount].GetComponent<StageController>().StartStage();

        nStageCount++;
    }
}
