using UnityEngine;

public class TestObject : MonoBehaviour
{
    [SerializeField]
    private float fAliveTime = 0.0f;

    private float fTimer = 0.0f;

    private void Start()
    {
        MyDebugLib.MessageLog(fAliveTime);

    }
    // Update is called once per frame
    void Update()
    {
        fTimer += Time.deltaTime;
        if(fTimer > fAliveTime)
        {
            MyDebugLib.MessageLog("Dead");
            Dead();
            fTimer = 0.0f;
        }
    }

    private void Dead()
    {
        transform.parent.GetComponent<GeneratorTest>().Release(this.gameObject);
    }
}
