using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private bool bIsConfiramation = false;
    private bool bIsExperience = false;
    private bool bIsComplete = false;

    private void Start()
    {
        StartCoroutine(Confirmation());
    }

    private void Update()
    {
       if(bIsComplete)
        {

        }
    }

    IEnumerator Confirmation()
    {
        if (bIsConfiramation) yield break;
        bIsConfiramation = true;

        // チュートリアル処理


        StartCoroutine(Experience());
        yield return null;
    }

    IEnumerator Experience()
    {
        if(bIsExperience) yield break;
        bIsExperience = true;

        // チュートリアル処理

        bIsComplete = true;
        yield return null;
    }

}
