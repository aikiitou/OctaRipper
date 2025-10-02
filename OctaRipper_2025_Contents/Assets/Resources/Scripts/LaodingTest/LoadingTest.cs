using UnityEngine;

public class LoadingTest : MonoBehaviour
{
    float fTimer = 0.0f;
    bool isLoading = false;

    // Update is called once per frame
    void Update()
    {
        fTimer += Time.deltaTime;

        if(fTimer > 1.0f)
        {
            if (isLoading) return;
            StartCoroutine(Loading.LoadScene("Title",this.gameObject));
            isLoading = true;
        }
    }
}
