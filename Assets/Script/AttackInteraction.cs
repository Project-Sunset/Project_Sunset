using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInteraction : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("Ѫ��")]
    public int health;


    [Header("��Ч")]
    [Tooltip ("���������Ч�¼�")]
    public GameObject[] dir_Effects;
    [Tooltip("�޷������Ч�¼�")]
    public GameObject[] Effects;


    [Header("���")]
    public bool is_shake; //���屻����ʱ��Ļ�Ƿ���
    private GameObject cam;

    void Awake()
    {
        //��ȡ��ǰ���������
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

        //�ж�����Ƿ��𶯣�ͬΪ��ʱ��
        //is_shake���ڸ���   ��������  �ж�����Ƿ�Ӧ����
        //should_shake���ڸ���  ������  �ж�����Ƿ�Ӧ����
        if (is_shake &&should_shake )
            cam.GetComponent<CamShake>().CameraShake(0.2f, 0.1f);

        health -= damage;
    }
}
