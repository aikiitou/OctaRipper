using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading
{
    private const float FADE_TIME = 1.0f;
    private const float ALPHA_MIN = 0.0f;
    private const float ALPHA_MAX = 1.0f;
    private const float MIN_LOAD_TIME = 1.0f;

    private static Canvas uCanvas = null;
    private static Image uFadeImage = null;

    private static void Initialize()
    {
        if (uCanvas != null)
        {
            return;
        }

        // キャンバスの生成
        GameObject canvasObject = new GameObject("LoadingCanvas");
        uCanvas = canvasObject.AddComponent<Canvas>();
        uCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uCanvas.sortingOrder = 10000;

        // フェード用のImageの生成
        GameObject fadeImageObject = new GameObject("FadeImage");
        fadeImageObject.transform.SetParent(canvasObject.transform);
        uFadeImage = fadeImageObject.AddComponent<Image>();
        uFadeImage.color = new Color(0,0,0,0);

        RectTransform rectTransform = uFadeImage.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.one;

        Object.DontDestroyOnLoad(canvasObject);
    }

    public static IEnumerator LoadScene(string scene_name,GameObject coroutine_start_object)
    {
        Initialize();
        Object.DontDestroyOnLoad (coroutine_start_object);
        float timer = 0.0f;

        MyDebugLib.MessageLog("FadeOut");
        yield return Fade(ALPHA_MIN,ALPHA_MAX);

        MyDebugLib.MessageLog("LoadStart");
        AsyncOperation async = SceneManager.LoadSceneAsync(scene_name);
        async.allowSceneActivation = false;

        while (true)
        {
            timer += Time.deltaTime;

            if (async.progress >= 0.9f && timer >= MIN_LOAD_TIME)
            {
                break;
            }

            yield return null;
        }
        MyDebugLib.MessageLog("ChangeScene");
        async.allowSceneActivation = true;

        yield return new WaitForSeconds(0.5f);

        yield return Fade(ALPHA_MAX,ALPHA_MIN);
        MyDebugLib.MessageLog("FadeIn");

        Object.Destroy(coroutine_start_object);
    }

    private static IEnumerator Fade(float fade_lerp_from,float fade_lerp_to)
    {
        float timer = 0.0f;
        while(timer < FADE_TIME)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fade_lerp_from, fade_lerp_to, timer / FADE_TIME);
            uFadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

    }
}
