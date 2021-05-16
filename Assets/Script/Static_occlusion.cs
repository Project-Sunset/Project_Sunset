using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Static_occlusion : MonoBehaviour
{
    private SpriteRenderer spr;
    private float sprite_width_half;
    public float adjust_h;


    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        sprite_width_half = spr.bounds.size.x * 0.5f;
    }

    // Update is called once per frame
#if UNITY_EDITOR
    void LateUpdate()
    {
        if(!Application.isPlaying)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, spr.bounds.center.y+ adjust_h);
        
    }
#endif

    private void OnDrawGizmos()
    {
        //Vector3 line = new Vector3(spr.transform.position.x, spr.transform.position.y + adjust_h, spr.transform.position.z);
        Vector3 line = new Vector3(spr.bounds.center.x, spr.bounds.center.y + adjust_h, spr.bounds.center.z);
        Gizmos.DrawLine(line + Vector3.left * sprite_width_half, line - Vector3.left * sprite_width_half);
    }
}
