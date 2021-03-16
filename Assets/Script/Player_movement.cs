using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public Transform sign_pos;//�ڵ���ǵ�
    public int health;
    public float run_speed;//�ܶ��ٶ�
    public float attack_speed;//����ʱ���ƶ��ٶ�
    public float rush_speed;//���ʱ���ƶ��ٶ�
    public int attack_damage;

    private float rb_speed;//�������õ������ϵ��ٶ�
    private Rigidbody2D rb;
    private Animator animator;

    private float inputX, inputY;//��¼����������ƶ���������ʱ�Զ���0
    private float stopX, stopY;//��¼�������޸ĵ��������
    private Vector2 moveDirection;//��¼���յ�λ�Ʒ�����Ҫ��λ��

    private Vector3 mousePostion;
    private float mouse2player_tx;
    private float mouse2player_ty;


    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePostion = Input.mousePosition;
        mousePostion = Camera.main.ScreenToWorldPoint(mousePostion);
        


        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(inputX, inputY).normalized;
        if(moveDirection!=Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = inputX;
            stopY = inputY;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        
        if (Input.GetButtonDown("Rush"))
        {
            mouse2player_tx = mousePostion.x - transform.position.x;
            mouse2player_ty = mousePostion.y - transform.position.y;
            animator.SetTrigger("Rush");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Judge_mouse_pos();
            animator.SetTrigger("Attack");
            
        }
        
        





        //���ݵ�ǰ��������rb_speed

        rb_speed = run_speed;//Ĭ��Ϊ�ܲ��ٶ�
        AnimatorStateInfo now_cilp;
        now_cilp = animator.GetCurrentAnimatorStateInfo(0);
        if(now_cilp .IsName("attack up")|| now_cilp.IsName("attack down") || now_cilp.IsName("attack_left") || now_cilp.IsName("attack_right"))
        {
            rb_speed = attack_speed;
            moveDirection = new Vector2(mouse2player_tx, mouse2player_ty).normalized;//����ʱ����귽���ƶ�
            stopX = (mouse2player_tx > 0) ? (mouse2player_tx + 1.5f) : (mouse2player_tx - 1.5f);
            stopY = mouse2player_ty;
        }
        if (now_cilp.IsName("rush left"))
        {
            rb_speed = rush_speed;
            moveDirection = new Vector2(mouse2player_tx, mouse2player_ty).normalized;//����ʱ����귽���ƶ�
            
            stopX = (mouse2player_tx > 0) ? (mouse2player_tx + 1.5f) : (mouse2player_tx - 1.5f);
            stopY = mouse2player_ty;
        }
        animator.SetFloat("InputX", stopX);
        animator.SetFloat("InputY", stopY);

    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * rb_speed;
    }




    /************************************Animation events�����¼�***********************************************************/
    public GameObject step_anim;
    void Step_event_left()//�Ų�
    {
        Instantiate(step_anim, new Vector3(sign_pos.position.x - 0.3f, sign_pos.position.y, sign_pos.position.z),
            Quaternion.identity);
    }

    void Step_event_right()
    {
        Instantiate(step_anim, new Vector3(sign_pos.position.x + 0.3f, sign_pos.position.y, sign_pos.position.z),
            Quaternion.identity);
    }



    public Transform[] attackPos;
    public float attackRange_x, attackRange_y;
    public LayerMask whatIsEnemies;
    void attack_up()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos[0].position, new Vector2(attackRange_x, attackRange_y), 0, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }
    void attack_down()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos[1].position, new Vector2(attackRange_x, attackRange_y), 0, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }
    void attack_left()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos[2].position, new Vector2(attackRange_x, attackRange_y), 0, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }
    void attack_right()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos[3].position, new Vector2(attackRange_x, attackRange_y), 0, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Enemy>().be_attacked(attack_damage, transform);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for(int i=0;i<attackPos.Length;i++)
            Gizmos.DrawWireCube(attackPos[i].position, new Vector3(attackRange_x, attackRange_y, 1));

    }

    public GameObject rush_start_effect;
    public GameObject rush_end_effect;
    public GameObject rush_effect_circle;

    void rush_start()//��ʼ���
    {
        if(mouse2player_tx>0)
            transform.localScale = new Vector3(-1, 1, 1);

        Instantiate(rush_start_effect, new Vector3(transform.position.x, transform.position.y, transform .position .z-0.1f),
            Quaternion.identity);
    }

    void rush_rotate()
    {
        transform.localScale = new Vector3(1, 1, 1);

        Vector2 v2 = new Vector2(mouse2player_tx, mouse2player_ty).normalized;
        float m = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

        transform.rotation=Quaternion.Euler(new Vector3(0, 0, m+180));


        Instantiate(rush_effect_circle, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f),
            Quaternion.Euler(new Vector3(0, 0, m + 180)));
    }

    void rush_rotate_back()
    {
        if (mouse2player_tx > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        transform.rotation = Quaternion.identity;
        //Instantiate(rush_end_effect, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f),
        //    Quaternion.identity);
        

    }

    void rush_over()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    /**************************************************************************************************************************/


    void Judge_mouse_pos()
    {
        mouse2player_tx = mousePostion.x - transform.position.x;
        mouse2player_ty = mousePostion.y - transform.position.y;

        if (Mathf.Abs(mouse2player_ty) > (Mathf.Abs(mouse2player_tx)+1.5f))
        {
            if (mouse2player_ty > 0)
                animator.SetTrigger("Mouse_up");
            else
                animator.SetTrigger("Mouse_down");
        }
        else
        {
            if (mouse2player_tx > 0)
                animator.SetTrigger("Mouse_right");
            else
                animator.SetTrigger("Mouse_left");
        }
    }

    //special effects and camera shake
    public GameObject[] dir_Effects;
    public GameObject[] Effects;
    public bool be_attacked(int damage, Transform attacker)
    {
        AnimatorStateInfo now_cilp;
        now_cilp = animator.GetCurrentAnimatorStateInfo(0);
        if (now_cilp.IsName("rush left"))
        {
            return false;
        }
        else
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
            cam.GetComponent<CamShake>().CameraShake(0.2f, 0.1f);
            return true;
        }
            
    }
}

