using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private enum State { idle, run, attack, block, roll, die}
    private State state = State.idle;

    [SerializeField] private int health = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        float hDirection = Input.GetAxis("Horizontal");
        float vDirection = Input.GetAxis("Vertical");
        float Attack = Input.GetAxis("Fire1");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            state = vDirection < 0 ? State.roll : State.run;
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(5, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            state = vDirection < 0 ? State.roll : State.run;
        }
        else
        {
            state = State.idle;
        }


        if (Attack != 0)
        {
            state = State.attack;
        }


        // jump
        /*
        if(vDirection > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 10f);
            state = State.jump;
        }
        */
        anim.SetInteger("state", (int)state);
    }


    /*
    public void OnCollisionEnter(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
        }
    }
    */
}
