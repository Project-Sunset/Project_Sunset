using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    private Animator anim;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.transform.position.z-100);
    }

    void stop_shake()
    {
        anim.SetBool("shake", false);
    }
}
