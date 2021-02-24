using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject[] dir_Effects;
    public GameObject[] Effects;
    public Animator camani;

    public virtual void be_attacked(int damage,Transform attacker)
    {
        Vector2 v2 = (new Vector2(attacker.position.x, attacker.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
        float m = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

        for(int i=0;i<dir_Effects.Length;i++)
        {
            Instantiate(dir_Effects[i], new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, attacker.position.z) - 0.1f),
            Quaternion.Euler(new Vector3(0, 0, m + 90)));
        }

        for(int i=0;i<Effects.Length;i++)
        {
            Instantiate(Effects[i], new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, attacker.position.z) - 0.1f),
            Quaternion.identity);
        }

        camani.SetBool("shake", true);
        health -= damage;
    }

}
