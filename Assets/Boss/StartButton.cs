using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] AudioClip startSound;

    //�X�^�[�g�{�^��

    public void StartButtonPush()
    {
        GetComponent<AudioSource>().PlayOneShot(startSound);
        SceneManager.LoadScene("SampleScene");
    }
}
