using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_control : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]private Material material;
    public float run_speed;
    public float rush_speed;
    private float teleportSpeed;
    public Transform sign_pos;
    public int health;
    public int attack_damage;
    private CamShake camShake;

    float speed;
    Vector3 mousePostion;
    float Verticalmove;
    float Horizontalmove;
    float mouse2player_tx;
    float mouse2player_ty;
    bool ready2rush=false;
    bool ready2teleport=false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
//player movement
        if(ready2rush ==false )
        {
            mousePostion = Input.mousePosition;
            mousePostion = Camera.main.ScreenToWorldPoint(mousePostion);
            mouse2player_tx = mousePostion.x - transform.position.x;
            mouse2player_ty = mousePostion.y - transform.position.y;
        }
        if(ready2teleport)
        {
            teleportSpeed=Mathf.Clamp01(teleportSpeed+Time.deltaTime);
            material.SetFloat("_teleportSpeed", teleportSpeed);
        }
        else
        {
            teleportSpeed = Mathf.Clamp01(teleportSpeed-Time.deltaTime);
            material.SetFloat("_teleportSpeed", teleportSpeed);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ready2teleport = true;
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ready2teleport = false;
        }
        if (Mathf.Abs(mouse2player_ty) > Mathf.Abs(mouse2player_tx))
        {
            if (mouse2player_ty > 0)
            {
                anim.SetBool("mouse up", true);
                anim.SetBool("mouse down", false);
            }
            else
            {
                anim.SetBool("mouse up", false);
                anim.SetBool("mouse down", true);
            }
        }
        else
        {
            anim.SetBool("mouse up", false);
            anim.SetBool("mouse down", false);
        }

        if(Input.GetButtonDown("Rush"))
        {
            ready2rush = true;
        }


        if (anim.GetBool("attack")||ready2rush ==true)
        {
            if (anim.GetBool("attack"))
            {
                speed = 0;
            }
            else
            {
                
                anim.SetBool("rush", true);
                Horizontalmove = mouse2player_tx;
                Verticalmove = mouse2player_ty;
                if (mouse2player_tx > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        else
        {
            speed = run_speed;
            Verticalmove = Input.GetAxisRaw("Vertical");
            Horizontalmove = Input.GetAxisRaw("Horizontal");
            if (Verticalmove > 0)
            {
                anim.SetBool("up", true);
                anim.SetBool("down", false);
            }
            else
            {
                if (Verticalmove < 0)
                {
                    anim.SetBool("up", false);
                    anim.SetBool("down", true);
                }
                else
                {
                    anim.SetBool("up", false);
                    anim.SetBool("down", false);
                }
            }
            if (Horizontalmove != 0)
            {
                if (Horizontalmove > 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);
                anim.SetBool("run", true);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                anim.SetBool("run", false);
            }
//judge attack
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetBool("attack", true);
                
                if (mouse2player_tx > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Horizontalmove , Verticalmove ).normalized * speed;
    }




/************************************Animation events***********************************************************/

    void attack_over()
    {
        anim.SetBool("attack", false);
    }

//judge whether emeny will be hit
    public Transform attackPos;
    public Transform attack_up_Pos;
    public Transform attack_down_Pos;
    public float attackRange_x, attackRange_y;
    public LayerMask whatIsEnemies;
    //void attack()
    //{
    //    Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange_x, attackRange_y), 0,whatIsEnemies);
    //    for(int i=0;i<enemiesToDamage.Length;i++)
    //    {
    //        enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
    //    }
    //}
    void attack_up()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attack_up_Pos.position, new Vector2(attackRange_y, attackRange_x), 0, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }
    void attack_down()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attack_down_Pos.position, new Vector2(attackRange_y, attackRange_x), 0, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }


    public GameObject step_anim;
    void Step_event_left()
    {
        Instantiate(step_anim, new Vector3(sign_pos.position.x-0.3f, sign_pos.position.y, sign_pos.position.z),
            Quaternion.identity);
    }

    void Step_event_right()
    {
        Instantiate(step_anim, new Vector3(sign_pos.position.x + 0.3f, sign_pos.position.y, sign_pos.position.z),
            Quaternion.identity);
    }


    void rush_over()
    {
        ready2rush = false;
        anim.SetBool("rush", false);
    }

    public GameObject rush_start_effect;
    void rush_start()
    {
        speed = 0;
    }

    void rush_change2black()
    {
        speed = rush_speed;
    }

    void rush_change2normal()
    {
        speed = 0;
    }
    /************************************************************************************************************/





    //assist visual positioning
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRange_x, attackRange_y, 1));
        Gizmos.DrawWireCube(attack_down_Pos.position, new Vector3(attackRange_y, attackRange_x, 1));
        Gizmos.DrawWireCube(attack_up_Pos.position, new Vector3(attackRange_y, attackRange_x, 1));
    }




//special effects and camera shake
    public GameObject[] dir_Effects;
    public GameObject[] Effects;
    public void be_attacked(int damage, Transform attacker)
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
        camShake.CameraShake(0.1f,0.1f);
        health -= damage;
    }
}
