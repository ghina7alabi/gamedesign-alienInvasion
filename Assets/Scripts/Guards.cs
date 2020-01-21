using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guards : MonoBehaviour
{

    public GameObject guardPrefab;
    

    public int attackSpeed;

    public float maxValue;
    public float minValue;
    public float distance;
    float currentValue;
    public float walkSpeed = 1;
    float xScale = -0.7449265f;

    public Transform player;
    Animator animator;

    //states
    const int idleState = 0;
    const int hurtState = 1;
    const int attackState = 2;
    const int walkState = 4;

    //bool
    bool isHurt = false;
    bool isAttacking = false;
    bool isWalking = false;

    bool normal = true;

    int currentAnimationState = idleState;

    //scores
    int lives = 3;
    public static int score = 0;
    private static int movingGuardsScore = 0;

    public Text scoreText;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        changeState(walkState);
        movingGuardsScore = NormalGuard.score;
        currentValue = transform.position.x;
        maxValue = transform.position.x;
        minValue = transform.position.x - distance;
    }

    void FixedUpdate()
    {
        movingGuardsScore = NormalGuard.score;

        scoreText.text = "Damage done: " + (score + movingGuardsScore);

        //movement
        if (normal)
        { 
            currentValue += Time.deltaTime * walkSpeed;
            if (currentValue >= maxValue)
            {
                walkSpeed *= -1;
                xScale *= -1;
                currentValue = maxValue;
            }
            else if (currentValue <= minValue)
            {
                walkSpeed *= -1;
                xScale *= -1;
                currentValue = minValue;
            }
            transform.position = new Vector3(currentValue, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            transform.localScale = new Vector3(xScale, 0.7449265f, 0.7449265f);
        }
        
        //check if any animation is running
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt sugarcane"))
        { isHurt = true;
            normal = false;
        }
        else
        { isHurt = false; }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack sugarcane"))
        {
            isAttacking = true;
            normal = false;
            transform.position = Vector3.MoveTowards(transform.position, player.position, attackSpeed * Time.deltaTime);
        }
        else
        { isAttacking = false; }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk sugarcane"))
        {
            isWalking = true;
        }
        else
        { isWalking = false; }

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
                break;

            case walkState:
                animator.SetInteger("state", walkState);
                break;
                
        }

        currentAnimationState = state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            lives -= 1;
            iTween.ShakePosition(Camera.main.gameObject, new Vector3(this.transform.position.x, 0, 0), 0.25f);
            changeState(attackState);

            if (lives <= 0)
            {
                Destroy(this.gameObject, 0);
                score += 5;
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            changeState(attackState);
            transform.position = Vector3.MoveTowards(transform.position, player.position, attackSpeed * Time.deltaTime);
        }

    }
    

}




