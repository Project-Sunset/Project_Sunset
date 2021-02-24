using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject darkEffect;
    public GameObject darkEffect_ex;
    public GameObject Effect_anim;
    public Animator camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }



public void be_attacked(int damage,Transform player)
    {
        Vector2 v2 = (new Vector2(player.position.x, player.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
        float m = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;

        Instantiate(darkEffect, new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, player.position.z) -0.1f), 
            Quaternion.Euler(new Vector3(0, 0, m+90)));

        Instantiate(darkEffect_ex, new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, player.position.z) - 0.1f),
            Quaternion.identity);
        Instantiate(Effect_anim, new Vector3(transform.position.x, transform.position.y, Mathf.Min(transform.position.z, player.position.z) - 0.1f),
            Quaternion.identity);
        camera.SetBool("shake", true);
        health -= damage;
    }

    void Dead()
    {
        if(health<=0)
        {
            Destroy(gameObject);
        }
    }
}
