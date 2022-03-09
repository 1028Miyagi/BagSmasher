using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] Button retry;
    [SerializeField] Button gameEnd;
    [SerializeField] Text resultTimeText;
    [SerializeField] float timeLimit = 300f;

    public float bottonSelect;
    float resultTime;
    float clearTime;

    [SerializeField] AudioSource ResultAudioSource;
    [SerializeField] AudioClip[] ResultSound;
    [SerializeField] float[] ResultSoundVolume;

    public bool canPut = true;

    void Start()
    {
        resultTime = Timer.getTime();
        
        if (resultTime == 0)
        {
            ResultAudioSource.volume = ResultSoundVolume[1];
            ResultAudioSource.PlayOneShot(ResultSound[1]);
        }
        else
        {
            ResultAudioSource.volume = ResultSoundVolume[0];
            ResultAudioSource.PlayOneShot(ResultSound[0]);
        }
        retry.Select();
    }

    void Update()
    {
        if (resultTime == 0)
        {
            
        }
        else
        {
            clearTime = timeLimit - resultTime;
            resultTimeText.text = clearTime.ToString("f2");
        }

        #region　ボタン

        bottonSelect = Input.GetAxisRaw("Horizontal");

        if (bottonSelect == 1)
        {
            gameEnd.Select();
        }

        //スティックなどで選択しているボタンを変更する時の音

        if((bottonSelect != 0) && canPut)
        {
            ResultAudioSource.volume = ResultSoundVolume[2];
            ResultAudioSource.PlayOneShot(ResultSound[2]);
            
            canPut = false;
        }
        else if(bottonSelect == 0)
        {
            canPut = true;
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
