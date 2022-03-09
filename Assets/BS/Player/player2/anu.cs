using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anu : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("isRun");
        }
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJump");
        }
    }
}
