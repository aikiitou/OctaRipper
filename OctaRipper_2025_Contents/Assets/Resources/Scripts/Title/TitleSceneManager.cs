using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{

    public void ChangeMainScene()
    {
        StartCoroutine(Loading.LoadScene("Main",this.gameObject));
    }

    public void ChangeTutorialScene()
    {
        StartCoroutine(Loading.LoadScene("Tutorial", this.gameObject));
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
