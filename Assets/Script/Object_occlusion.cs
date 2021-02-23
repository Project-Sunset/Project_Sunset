using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_occlusion : MonoBehaviour
{
    public GameObject sign;
    public GameObject player;
    private float z;
    // Start is called before the first frame update
    void Start()
    {
        z = GetComponent<Transform>().position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (sign.transform.position.y > player.transform.position.y)
            transform.position = new Vector3(transform.position.x, transform.position.y, z+10);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
