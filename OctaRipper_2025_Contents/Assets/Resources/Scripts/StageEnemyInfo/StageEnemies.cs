using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageEnemy", menuName = "Scriptable Objects/StageEnemy")]
public class StageEnemies : ScriptableObject
{
    [System.Serializable]
    public struct StageEnemy
    {
        public string sEnemyTag;
        public int nStageTotal;
        public int nPopTotal;
    }

    [SerializeField]
    private List<StageEnemy> enemies;

    public List<StageEnemy> GetStageEnemies()
    {
        return enemies; 
    }
}
