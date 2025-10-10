using UnityEngine;

public class GameClearNotification : MonoBehaviour
{
    public void ChageGameClearGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(Loading.LoadScene("Clear", this.gameObject));
    }
}
