using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : EnemyAI
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("�ٶ�")]
    public float run_speed;//�����ٶ�
    public float attack_speed;//�����ٶ�

    public float rb_speed;//�������õ�rb�ϵ��ٶȴ�С
    private Vector2 rb_direction;//�������õ�rb�ϵ��ٶȷ���

    [Header("�����뾶")]
    public float attackRange;//������˷�Χʱ����

    [Header("׷���뾶")]
    public float pursuitRange;//����ҽ���˷�Χʱ�����˷���

    [Header("�������ʱ��")]
    public float attackInterval;//����������������һ�ι�������ǰ�ĵ���ʱ��
    private float attackIntervalTimer;//�������Եļ�ʱ��

    public LayerMask Obstacle;
    // Start is called before the first frame update
    private void Start()
    {
        attackIntervalTimer = Time.time- attackInterval-1f;
        rb =GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AIStart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health>0)
        {
            rb_direction = ActionJudge().normalized;
            if (rb_direction.x < 0f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            Vector2 velocity_0 = rb.velocity;
            Vector2 velocity_1 = rb_speed * rb_direction;
            rb.velocity = Vector2.Lerp(velocity_0, velocity_1, 0.2f);
        }
        else
        {
            animator.SetBool("die", true);
        }

    }

    Vector2 ActionJudge()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        RaycastHit2D raycastObstacle = Physics2D.Raycast(transform.position, target.position - transform.position, Mathf.Min(pursuitRange, distance), Obstacle);
        //Debug.DrawRay(transform.position, target.position - transform.position, Color.red, pursuitRange);//��������
        AnimatorStateInfo now_cilp;
        now_cilp = animator.GetCurrentAnimatorStateInfo(0);
        if(now_cilp.IsName("attack"))
        {
            //print("attack");
            rb_speed = attack_speed;
            return rb_direction;
        }
        if (now_cilp.IsName("ready2attack"))
        {
            //print("attack");
            rb_speed = 0;
            return target.position - transform.position;
        }



        if (Time.time - attackIntervalTimer < attackInterval)
        {
            //print("runaway");
            rb_speed = run_speed;
            return (transform.position - target.position);
        }
        else
        {
            if (distance < pursuitRange)
            {
                if(raycastObstacle)
                {
                    rb_speed = run_speed;
                    return GetDirfromPath();
                }
                else
                {
                    if (distance < attackRange)
                    {
                        //print("attack");
                        animator.SetTrigger("attack");
                        rb_speed = 0f;
                        return target.position - transform.position;
                    }
                    else
                    {
                        //print("run");
                        rb_speed = run_speed;
                        return target.position - transform.position;
                    }
                }

            }
            else
            {
                //print("idle");
                rb_speed = run_speed;
                return Vector2.zero;
            }
        }

    }


    void AttackOver()
    {
        attackIntervalTimer = Time.time;
    }
}
