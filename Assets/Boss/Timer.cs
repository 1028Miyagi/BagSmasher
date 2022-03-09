using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeLimit = 300f; //制限時間
    public static float remainingTime; //現在の時間
    float waitTime = 0.0f;

    public int stoper; //タイマーを止める用

    bool getBossFlag;
    bool getPlayerFlag;

    Text timerText;

    //タイマー、クリア/ゲームオーバー関連

    public static float getTime()
    {
        return remainingTime;
    }

    void Start()
    {
        timerText = GetComponent<Text>();
        remainingTime = timeLimit;
        stoper = 1;
        waitTime = 0.0f;
    }

    void Update()
    {
        if (stoper == 1)
        {
            remainingTime -= Time.deltaTime;
        }

        getBossFlag = BossHP.SetBossDeathFlag();

        if (getBossFlag) //ボスを倒した時
        {
            stoper = 0;
            remainingTime -= Time.deltaTime * stoper;
            waitTime += Time.deltaTime;

            if (waitTime >= 3.0f)
            {
                SceneManager.LoadScene("Result");
            }
        }

        getPlayerFlag = Player.SetPlayerDeathFlag();

        if (getPlayerFlag || remainingTime <= 0) //時間切れ、またはプレイヤーが倒れた時
        {
            stoper = 0;
            remainingTime = 0;
            timerText.text = ("0.00");
            waitTime += Time.deltaTime;

            if (waitTime >= 1.0f)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        timerText.text = remainingTime.ToString("f2");
    }
}