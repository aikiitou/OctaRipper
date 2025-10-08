using UnityEngine;

public class FireWallController : MonoBehaviour
{
    [SerializeField]
    private float fStartLife;

    private float fCurrentLife;

    private void Awake()
    {
        fCurrentLife = fStartLife;
    }

    // Update is called once per frame
    void Update()
    {
        MyDebugLib.MessageLog("FWCL" +  fCurrentLife);
        if(fCurrentLife <= 0.0f)
        {
            StageClear();
        }
    }

    public void TakeDamage(float _damage)
    {
        MyDebugLib.MessageLog("FW:DAMEGE");
        fCurrentLife += _damage;
    }

    private void StageClear()
    {
        transform.parent.GetComponent<StageController>().ClearEnemies();
        gameObject.SetActive(false);
    }
}
