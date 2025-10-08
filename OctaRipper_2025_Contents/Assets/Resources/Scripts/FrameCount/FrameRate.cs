using System.Security.Cryptography.X509Certificates;
using NUnit;
using UnityEditor.Build;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    public static FrameRate instance;   //インスタンス化
    private int nFrameRate;     //現在のフレーム
    private int nFrame = 59;    //フレームをリセットする数

    private void Awake()
    {
        //インスタンス化
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
