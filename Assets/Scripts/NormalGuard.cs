using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalGuard : MonoBehaviour
{
    public Transform player;

    public int attackSpeed;

    Animator animator;

    const int idleState = 0;
    const int hurtState = 1;
    const int attackState = 2;

    bool isHurt = false;
    bool isAttacking = false;

    int currentAnimationState = idleState;

    int lives = 3;
    public static int score = 0;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        transform.position = Vector3.MoveTowards(transform.position, player.position, attackSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        //check if any animation is running
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt sugarcane"))
        { isHurt = true; }
        else
        { isHurt = false; }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack sugarcane"))
        {
            isAttacking = true;
            transform.position = Vector3.MoveTowards(transform.position, player.position, attackSpeed * Time.deltaTime);

        }
        else
        { isAttacking = false; }

        

    }



    void changeState(int state)
    {

        if (currentAnimationState == state)
            return;

        switch (state)
        {
            case idleState:
                animator.SetInteger("state", idleState);
                break;

            case hurtState:
                animator.SetInteger("state", hurtState);
                break;

            case attackState:
                animator.SetInteger("state", attackState);
                transform.position = Vector3.MoveTowards(transform.position, player.position, attackSpeed * Time.deltaTime);
                break;

                
                

        }

        currentAnimationState = state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            lives -= 1;
            changeState(attackState);

            if (lives <= 0)
            {
                score += 5;
                Destroy(this.gameObject);

            }
        }
        
    }

   



}



