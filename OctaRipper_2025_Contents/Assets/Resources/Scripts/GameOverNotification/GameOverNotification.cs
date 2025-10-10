using UnityEngine;

public class GameOverNotification : MonoBehaviour
{
    public void ChageGameOverGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(Loading.LoadScene("GameOver",this.gameObject));
    }
}
