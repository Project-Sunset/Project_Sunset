using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : AttackInteraction
{

    //参考自https://www.bilibili.com/video/BV1D4411N7FZ

    [Header("追击目标")]
    public Transform target;

    [Header("下一路径点距离，距离小于该值时则判断为到达")]
    public float nextWaypointDistance = 1f;

    //是否到达
    private bool reachedEndOfPath = false;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;


    protected void AIStart()
    {
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath((Vector2)transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    //根据当前位置pos获取追击方向，返回值为二维单位向量
    public Vector2 GetDirfromPath()
    {
        if (path == null)
            return new Vector2(0f,0f);

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return new Vector2(0f, 0f);
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        return direction;
    }
}
