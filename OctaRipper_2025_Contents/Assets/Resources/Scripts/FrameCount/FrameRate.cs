using NUnit;
using UnityEditor.Build;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    private int nFrameRate;     //現在のフレーム
    private int nFrame = 59;    //フレームをリセットする数

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        nFrameRate++;
        if (nFrameRate > nFrame)
        {
            nFrameRate = 0;
        }
    }
    public int GetFPS()
    {
        return nFrameRate;
    }
}
