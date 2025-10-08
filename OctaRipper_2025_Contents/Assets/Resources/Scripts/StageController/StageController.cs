using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageController : MonoBehaviour
{
    private const float RANDOM_RANGE_HALF = 20.0f;
    private const float E_TO_P_RANGE = 5.0f;
    private const float RANDOM_ATTEMPTS_LIMIT = 10;

    [SerializeField]
    private StageEnemies cStageEnemies = null;
    [SerializeField]
    private GameObject gFireWall = null;

    private List<GameObject> gEnemies = new List<GameObject>();
    private List<bool> bPrevStates = new List<bool>();
    private Coroutine cWatcher;

    private bool bStageClear = false;

    private Dictionary<string, int> cPopCount = new Dictionary<string, int>();    

    private void OnDisable()
    {
        if (cWatcher != null) StopCoroutine(cWatcher);
    }

    public void StartStage()
    {
        var stageEnemies = cStageEnemies.GetStageEnemies();
        foreach (var enemyData in stageEnemies)
        {
            for(int i = 0; i < enemyData.nStageTotal; i++)
            {
                AppearanceEnemy(enemyData.sEnemyTag);
            }
        }
        cWatcher = StartCoroutine(WatchCoroutine());
    }

    private IEnumerator WatchCoroutine()
    {
        while (true)
        {
            if (bPrevStates.Count != gEnemies.Count) SyncPrevStates();

            for (int i = 0; i < gEnemies.Count ; i++)
            {
                GameObject checkObject = gEnemies[i];
                bool prev = bPrevStates[i];
                bool current = IsActiveSafe(checkObject);

                if (prev && !current)
                {
                    EliminationEnemy(checkObject);
                }

                bPrevStates[i] = current;
            }

            yield return null;
        }
    }

    private void SyncPrevStates()
    {
        bPrevStates.Clear();
        foreach (var enemy in gEnemies)
        {
            bPrevStates.Add(IsActiveSafe(enemy));
        }
    }
        
    private bool IsActiveSafe(GameObject check_object)
    {
        if (check_object == null) return false;
        return check_object.activeSelf;
    }

    private void AppearanceEnemy(string _tag)
    {
        if(bStageClear)
        {
            return;
        }

        GameObject obj = transform.parent.GetComponent<StageManager>().RequestGetPoolObject(_tag);
        obj.transform.parent = transform;
        gEnemies.Add(obj);
        if (obj.tag == "Boss")
        {
            obj.transform.position = this.transform.position;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 setPosition = this.transform.position;
            int attemptsNum = 0;
            while (attemptsNum < RANDOM_ATTEMPTS_LIMIT)
            {
                setPosition = SetRandomPos(this.transform.position);

                float distance = Vector3.Distance(setPosition, player.transform.position);
                if(distance > E_TO_P_RANGE)
                {
                    break;
                }
                attemptsNum++;
            }
            obj.transform.position = setPosition;
        }
        obj.SetActive(true);

        int currentPopCount;
        cPopCount.TryGetValue(_tag,out currentPopCount);
        cPopCount[_tag] = currentPopCount + 1;
        
    }

    private Vector3 SetRandomPos(Vector3 base_position)
    {
        Vector3 returnVec = Vector3.zero;

        float posX = Random.Range(-RANDOM_RANGE_HALF, RANDOM_RANGE_HALF);
        float posz = Random.Range(-RANDOM_RANGE_HALF, RANDOM_RANGE_HALF);

        returnVec = new Vector3(transform.position.x + posX, base_position.y, transform.position.z + posz);

        return returnVec;
    }

    private IEnumerator RepopEnemy(string _tag)
    {
        if(cPopCount[_tag] > GetPopTotal(_tag))
        {
            yield break;
        }

        yield return new WaitForSeconds(3.0f);
        AppearanceEnemy(_tag);
        yield return null;
    }

    private int GetPopTotal(string _tag)
    {
        foreach (var enemy in cStageEnemies.GetStageEnemies())
        {
            if (enemy.sEnemyTag == _tag)
            {
                return enemy.nPopTotal;
            }
        }

        return 0;
    }

    public void ClearEnemies()
    {
        bStageClear = true;

        for (int i = 0; i < gEnemies.Count; i++)
        {
            gEnemies[i].SetActive(false);
            EliminationEnemy(gEnemies[i]);
        }

    }


    private void EliminationEnemy(GameObject game_object)
    {
        if(!bStageClear)
        {
            StartCoroutine(RepopEnemy(game_object.tag));
        }
        transform.parent.GetComponent<StageManager>().RequestReturnPoolObject(game_object);
    }
}
