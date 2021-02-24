using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//12312312132112132132

public class player_control : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float run_speed;
    public Transform sign_pos;
    public int health;

    float speed;
    Vector3 mousePostion;
    float Verticalmove ;//��ֱ�����ϵ��ƶ�
    float Horizontalmove ;//ˮƽ�����ϵ��ƶ�
    float mouse2player_tx; 
    float mouse2player_ty;

    public int attack_damage;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {


        if (anim.GetBool("attack"))
        {
            speed = 0;

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



            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetBool("attack", true);
                mousePostion = Input.mousePosition;
                mousePostion = Camera.main.ScreenToWorldPoint(mousePostion);
                mouse2player_tx = mousePostion.x - transform.position.x;
                mouse2player_ty = mousePostion.y - transform.position.y;
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



    void attack_over()
    {
        anim.SetBool("attack", false);
    }


    public Transform attackPos;
    public Transform attack_up_Pos;
    public Transform attack_down_Pos;
    public float attackRange_x, attackRange_y;
    public LayerMask whatIsEnemies;
    void attack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRange_x, attackRange_y), 0,whatIsEnemies);
        for(int i=0;i<enemiesToDamage.Length;i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRange_x, attackRange_y, 1));
        Gizmos.DrawWireCube(attack_down_Pos.position, new Vector3(attackRange_y, attackRange_x, 1));
        Gizmos.DrawWireCube(attack_up_Pos.position, new Vector3(attackRange_y, attackRange_x, 1));
    }


    public Animator camani;
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

        camani.SetBool("shake", true);
        health -= damage;
    }
}
