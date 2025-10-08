using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class FireWallController : MonoBehaviour
{
    [SerializeField]
    private float fStartLife;

    private float fCurrentLife;
    private StageController cStageController;

    private void Awake()
    {
        fCurrentLife = fStartLife;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cStageController = transform.parent.GetComponent<StageController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fCurrentLife < 0.0f)
        {
            StageClear();
        }
    }

    public void TakeDamage(float _damage)
    {
        fCurrentLife -= _damage;
    }

    private void StageClear()
    {
        transform.parent.GetComponent<StageController>().ClearEnemies();
    }
}
