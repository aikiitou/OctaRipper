using UnityEngine;

public class GameOverNotification : MonoBehaviour
{
    public void ChageGameOverGameOver()
    {
        StartCoroutine(Loading.LoadScene("GameOver",this.gameObject));
    }
}
