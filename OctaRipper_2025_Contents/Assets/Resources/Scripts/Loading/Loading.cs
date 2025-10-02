using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.SearchService;
using UnityEngine;

public class Loading : MonoBehaviour
{    
    public bool StartLoading(string scene_name)
    {
        bool returnValue = false;

        StartCoroutine(LoadScene(scene_name));

        return returnValue;
    }

    IEnumerator LoadScene(string scene_name)
    {
        yield return null;
    }
}
