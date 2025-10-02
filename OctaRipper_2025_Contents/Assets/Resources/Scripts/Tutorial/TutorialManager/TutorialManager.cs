using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    //操作説明のボード
    [SerializeField] GameObject[] gBoards;
    int nBoardIndex;                         //ボードのインデックス
    float fBoardCurrentAngle;                //ボードの現在の向き
    float fBoardTargetRotateUp = -90;        //ボードが起き上がるときのxの向き
    float fBoardTargetRotateDown = 0;        //ボードが倒れる時のxの向き
    [SerializeField] float boardDuration;   //ボードが起き上がる時間
    [SerializeField] float boardRotation;   //ボードが回る速度

    void Start()
    {

    }
    void Update()
    {
        BordController();
        TitleScene();
        MyDebugLib.MessageLog(nBoardIndex);
    }
    
    void BordController()//ボード操作全般
    {
        gBoards[nBoardIndex].transform.localEulerAngles = new Vector3(fBoardCurrentAngle, 180, 0);  //現在のボードの向き
        BoardAngle();
        if (nBoardIndex >= 4)
        {
            nBoardIndex--;
        }
    }
    void BoardAngle()//ボードの向きの変更
    {
        if (Input.GetKey(KeyCode.E))
        {//ボードが起き上がるとき
            float speed = boardRotation / boardDuration;
          fBoardCurrentAngle = Mathf.MoveTowards(fBoardCurrentAngle, fBoardTargetRotateUp, speed * Time.deltaTime);
       }
        else if (Input.GetKey(KeyCode.Q))
        {//ボードが倒れるとき
            float speed = boardRotation / boardDuration;
            fBoardCurrentAngle = Mathf.MoveTowards(fBoardCurrentAngle, fBoardTargetRotateDown, speed * Time.deltaTime);
            if (fBoardCurrentAngle >= 0)
            {
                nBoardIndex++;
            }
        }
    }
    void TitleScene()//タイトルシーン遷移
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
