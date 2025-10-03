using NUnit;
using UnityEditor.Build;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    private int nFrameRate;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        nFrameRate++;
        if (nFrameRate > 60)
        {
            nFrameRate = 0;
        }
        MyDebugLib.MessageLog(nFrameRate);
    }
    public int GetFPS()
    {
        return nFrameRate;
    }
}
