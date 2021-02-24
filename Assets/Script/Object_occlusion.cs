using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_occlusion : MonoBehaviour
{
    public GameObject sign;

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, sign.transform.position.y);

    }
}
