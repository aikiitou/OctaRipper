using UnityEngine;

public class ClearSceneManager: MonoBehaviour
{
    public void ChangeMainScene()
    {
        StartCoroutine(Loading.LoadScene("Main", this.gameObject));
    }

    public void ChangeTaitleScene()
    {
        StartCoroutine(Loading.LoadScene("Title", this.gameObject));
    }
}
