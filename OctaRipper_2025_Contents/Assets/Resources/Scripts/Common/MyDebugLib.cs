using UnityEngine;

public class MyDebugLib : MonoBehaviour
{
    public static void MessageLog(object _message)
    {
#if UNITY_EDITOR
        Debug.Log(_message);
#endif
    }
}
