using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolf : Enemy
{
    private Rigidbody2D rb;
    private Animator anim;

    public float Stiffness_time;
    public float run_speed;
    public float attack_speed;
    public Transform player;
    public float attack_range;
    public int attack_damage;

    int damage=0;
    float speed;
    float Verticalmove;
    float Horizontalmove;
    float Stiffness_timer;
    bool on_be_attacked=false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
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

    void Action_judgment()
    {
        float dis = (new Vector2(transform.position.x, transform.position.y) - new Vector2(player.position.x, player.position.y)).sqrMagnitude;
        if(dis<attack_range)
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
                enemiesToDamage[i].GetComponent<player_control>().be_attacked(damage, transform);
                damage = 0;
            }
            
        }

    }

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

            camani.SetBool("shake", true);
            health -= damage;
            on_be_attacked = true;
            Stiffness_timer = 0;
        }




    }
}
