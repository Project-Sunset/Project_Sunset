using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform playerPosition;

    float smoothAmount = 0.2f;

    private void FixedUpdate()
    {
        Vector3 processedPosition_1 = playerPosition.position;
        Vector3 processedPosition_2 = Vector3.Lerp(transform.position, processedPosition_1, smoothAmount);
        transform.position = processedPosition_2;
        //transform.LookAt(playerPosition);
    }
}
