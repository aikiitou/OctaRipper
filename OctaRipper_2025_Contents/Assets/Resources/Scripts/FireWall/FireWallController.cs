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
