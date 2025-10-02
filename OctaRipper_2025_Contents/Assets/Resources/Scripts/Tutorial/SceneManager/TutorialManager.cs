using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    //操作説明のボード
    [SerializeField] List<GameObject> boards = new List<GameObject>();//ボード
    int boardIndex;                         //ボードのインデックス
    float boardCurrentAngle;                //ボードの現在の向き
    float boardTargetRotateUp = -90;        //ボードが起き上がるときのxの向き
    float boardTargetRotateDown = 0;        //ボードが倒れる時のxの向き
    [SerializeField] float boardDuration;   //ボードが起き上がる時間
    [SerializeField] float boardRotation;   //ボードが回る速度

    void Start()
    {

    }
    void Update()
    {
        BordController();
        TitleScene();
        
    }
    void BordController()//ボード操作全般
    {
        foreach (GameObject board in boards)    //Listに入っているもの全てに実行
        {
            board.transform.localEulerAngles = new Vector3(boardCurrentAngle, 180, 0);  //現在のボードの向き
        }
        BoardAngle();
    }
    void BoardAngle()//ボードの向きの変更
    {
        if (Input.GetKey(KeyCode.E))
        {//ボードが起き上がるとき
            float speed = boardRotation / boardDuration;
            boardCurrentAngle = Mathf.MoveTowards(boardCurrentAngle, boardTargetRotateUp, speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {//ボードが倒れるとき
            float speed = boardRotation / boardDuration;
            boardCurrentAngle = Mathf.MoveTowards(boardCurrentAngle, boardTargetRotateDown, speed * Time.deltaTime);
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
