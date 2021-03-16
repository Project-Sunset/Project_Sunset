﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolf : Enemy
{
    public GameObject cam;

    private Rigidbody2D rb;
    private Animator anim;
    public Transform player;
    public AudioSource deadAudio;
    public float Stiffness_time;
    public float run_speed;
    public float attack_speed;
    public float attack_range;
    public int attack_damage;

    int damage=1;
    float speed;
    float Verticalmove;
    float Horizontalmove;
    float Stiffness_timer;
    bool on_be_attacked=false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        deadAudio.GetComponent<AudioSource>();
    }

    void Update()
    {
        die();
        if(anim.GetBool("dead"))
        {
            speed = 0;
        }
        else
        {
            if(anim.GetBool("die"))
            {
                //Here, speed is set to a number less than zero to produce the effect of backward when dying
                speed = -3;
            }
            else
            {
                if (Stiffness_timer >= Stiffness_time)
                {
                    on_be_attacked = false;
                }

                if (on_be_attacked)
                {
                    Stiffness_timer += Time.deltaTime;
                    speed = -2;
                }
                else
                {
                    if(anim.GetBool("attack"))
                    {
                        
                        attack();
                    }
                    else
                    {
                        Action_judgment();
                    }
                }
            }
        }      
    }

    void FixedUpdate()
    {
        if (Horizontalmove > 0)
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        else
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        rb.velocity = new Vector2(Horizontalmove, Verticalmove).normalized * speed;
    }

    //Judge whether the player enters the attack range, if so, attack the player, if not, chase the player
    void Action_judgment()
    {
        float dis = (new Vector2(transform.position.x, transform.position.y) - new Vector2(player.position.x, player.position.y)).sqrMagnitude;
        //"dis" is the distance between the player and the wolf. 
        //Here, "sqrMagnitude" is used to calculate the rough result, because it can save CPU performance cost

        if (dis<attack_range)
        {
            speed = 0;
            anim.SetBool("attack", true);
        }
        else
        {
            anim.SetBool("run", true);
            speed = run_speed;
        }
        Horizontalmove = player.position.x - transform.position.x;
        Verticalmove = player.position.y - transform.position.y;
    }

//judge whether enemy will hit player
    public Transform attackPos;
    public float attackRange_x, attackRange_y;
    public LayerMask whatIsEnemies;
    void attack()
    {
        if(damage>0)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange_x, attackRange_y), 0, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if(enemiesToDamage[i].GetComponent<Player_movement>().be_attacked(damage, transform));
                    damage = 0;
            }
            
        }

    }

//trigger movement
    void attack_start()
    {
        speed = attack_speed;
        damage = attack_damage;
    }
    void attack_end()
    {
        damage = 0;
    }
    void attack_over()
    {
        anim.SetBool("attack", false);
    }
    void die()
    {
        if(health<=0)
        {
            
            anim.SetBool("die", true);
  
        }
    }
    void dead()
    {
        anim.SetBool("dead", true);
    }

//special effects and camera shake
    public override void be_attacked(int damage, Transform attacker)
    {
        if(!anim.GetBool("die"))
        {
            Vector2 v2 = (new Vector2(attacker.position.x, attacker.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
            float m = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

            for (int i = 0; i < dir_Effects.Length; i++)
            {
                Instantiate(dir_Effects[i], new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, attacker.position.z) - 0.1f),
                Quaternion.Euler(new Vector3(0, 0, m + 90)));
            }

            for (int i = 0; i < Effects.Length; i++)
            {
                Instantiate(Effects[i], new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, attacker.position.z) - 0.1f),
                Quaternion.identity);
            }
            health -= damage;
            deadAudio.Play();
            on_be_attacked = true;
            Stiffness_timer = 0;
            cam.GetComponent<CamShake>().CameraShake(0.2f, 0.1f);
        }
    }
}
