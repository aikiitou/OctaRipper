using System.Collections;
using UnityEngine;

public class PlayerDodgeEffectController : MonoBehaviour
{
    [SerializeField]
    float fEffectTime;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public IEnumerator DodgeEffect()
    {
        gameObject.SetActive(true);

        yield return new WaitForSeconds(fEffectTime);

        gameObject.SetActive(false);
    }
}
