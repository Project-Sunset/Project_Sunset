using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Object_occlusion : MonoBehaviour
{

    public float adjust_h;

    // Update is called once per frame
#if UNITY_EDITOR
    void LateUpdate()
    {
        if(!Application.isPlaying)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y+ adjust_h);

    }
#endif

    private void OnDrawGizmos()
    {
        Vector3 line = new Vector3(transform.position.x, transform.position.y + adjust_h, transform.position.z);
        Gizmos.DrawLine(line + Vector3.left * 2f, line + Vector3.right * 2f);
    }
}
