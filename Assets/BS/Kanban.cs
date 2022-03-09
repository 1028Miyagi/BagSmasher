using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kanban : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    [SerializeField] float K_size = 0.5f;
    //[SerializeField] float interval = 0.5f;
    float time;
    float s;
    private void Update()
    {
        if (play)
        {
            if (enter)
            {
                /*
                time += Time.deltaTime;
                if (time > interval)
                {
                    play = false;

                    time = 0;
                }
                else
                {*/
                if (s < K_size)
                {
                    s += 0.1f;
                }
                //float s = Mathf.Lerp(0, 10, time / interval);
                Vector3 scale = new Vector3(s, s, s);

                spriteRenderer.transform.localScale = scale;
                //}
            }
            else
            {
                /*
                time += Time.deltaTime;
                if (time > interval)
                {
                    play = false;

                    time = 0;

                    spriteRenderer.enabled = false;
                }
                else
                {*/
                if (s > 0)
                {
                    s -= 0.1f;
                }
                else
                    spriteRenderer.enabled = false;
                //float s = Mathf.Lerp(10, 0, time / interval);
                Vector3 scale = new Vector3(s, s, s);
                spriteRenderer.transform.localScale = scale;
                //}
            }
        }
    }

    bool play = false;
    bool enter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
            play = true;

            spriteRenderer.enabled = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
            play = true;
        }
    }
}
