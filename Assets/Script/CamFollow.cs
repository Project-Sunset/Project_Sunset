using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform playerPosition;
    public Vector3 adjustPosition;
    float smoothAmount = 0.1f;

    private void LateUpdate()
    {
        Vector3 processedPosition_1 = playerPosition.position + adjustPosition;
        Vector3 processedPosition_2 = Vector3.Lerp(transform.position, processedPosition_1, smoothAmount);
        transform.position = processedPosition_2;
        //transform.LookAt(playerPosition);
    }
}
