using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : AttackInteraction
{

    //�ο���https://www.bilibili.com/video/BV1D4411N7FZ

    [Header("׷��Ŀ��")]
    public Transform target;

    [Header("��һ·������룬����С�ڸ�ֵʱ���ж�Ϊ����")]
    public float nextWaypointDistance = 1f;

    //�Ƿ񵽴�
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

    //���ݵ�ǰλ��pos��ȡ׷�����򣬷���ֵΪ��ά��λ����
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
