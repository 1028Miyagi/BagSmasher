using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    AnimatorStateInfo stateInfo;

    public int Player_State = 0;

    enum Action
    {
        Wait,//0
        Run,//1
        Jump//2
    }

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }

    // Update is called once per frame
    void Update()
    {
        State();
        Move();
    }

    void Move()
    {
        //ƒL[“ü—Í
        float horizontalkey = Input.GetAxisRaw("Horizontal");
        if (horizontalkey > 0 || horizontalkey < 0)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    void State()
    {
        if (stateInfo.IsName("wait"))
        {
            Player_State = (int)Action.Wait;
        }
        else if (stateInfo.IsName("run"))
        {
            Player_State = (int)Action.Run;
        }
        else if (stateInfo.IsName("jump"))
        {
            Player_State = (int)Action.Jump;
        }
    }
}
