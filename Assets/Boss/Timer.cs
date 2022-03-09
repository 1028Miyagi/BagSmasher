using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeLimit = 300f; //��������
    public static float remainingTime; //���݂̎���
    float waitTime = 0.0f;

    public int stoper; //�^�C�}�[���~�߂�p

    bool getBossFlag;
    bool getPlayerFlag;

    Text timerText;

    //�^�C�}�[�A�N���A/�Q�[���I�[�o�[�֘A

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

        if (getBossFlag) //�{�X��|������
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

        if (getPlayerFlag || remainingTime <= 0) //���Ԑ؂�A�܂��̓v���C���[���|�ꂽ��
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