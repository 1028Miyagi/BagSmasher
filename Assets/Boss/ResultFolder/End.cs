using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public AudioSource pushAudioSource;
    public AudioClip pushSound;
    public float pushSoundVolume;

    //�Q�[���I���{�^��

    public void EndButtonPush()
    {
        pushAudioSource.volume = pushSoundVolume;
        pushAudioSource.PlayOneShot(pushSound);

        SceneManager.LoadScene("Title");
    }
}