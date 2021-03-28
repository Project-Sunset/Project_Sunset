using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction : MonoBehaviour
{
    [Header("基础属性")]
    [Tooltip("血量")]
    public int health;


    [Header("特效")]
    [Tooltip ("带方向的特效事件")]
    public GameObject[] dir_Effects;
    [Tooltip("无方向的特效事件")]
    public GameObject[] Effects;


    [Header("相机")]
    public bool is_shake; //物体被攻击时屏幕是否震动
    private GameObject cam;

    void Awake()
    {
        //获取当前场景主相机
        cam = GameObject.Find("Main Camera");
    }


    public virtual void be_attacked(int damage, Transform attacker,bool should_shake)
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

        //判断相机是否震动，同为真时震动
        //is_shake用于根据   被攻击方  判断相机是否应该震动
        //should_shake用于根据  攻击方  判断相机是否应该震动
        if (is_shake &&should_shake )
            cam.GetComponent<CamShake>().CameraShake(0.2f, 0.1f);

        health -= damage;
    }
}
