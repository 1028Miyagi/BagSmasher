using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    public AudioSource pushAudioSource;
    public AudioClip pushSound;
    public float pushSoundVolume;

    //リトライボタン

    public void RetryButtonPush()
    {
        pushAudioSource.volume = pushSoundVolume;
        pushAudioSource.PlayOneShot(pushSound);

        SceneManager.LoadScene("SampleScene");
    }
}
