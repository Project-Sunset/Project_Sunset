using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public Camera mainCam;
    float shakeAmount=0;

    void Awake() 
    {
        if(mainCam==null)
        {
            mainCam=Camera.main;
        }
    }
//core, deside to start and halt the shaking
    public void CameraShake(float shakeAmt,float shakeLth)
    {
        shakeAmount=shakeAmt;
        InvokeRepeating("StartShake",0,0.01f);
        Invoke("StopShake",shakeLth);
    }
    void StartShake()
    {
        if(shakeAmount>0)
        {
            Vector3 cameraPosition=mainCam.transform.position;
            float Xaxis=Random.value*shakeAmount*2-shakeAmount;
            float Yaxis=Random.value*shakeAmount*2-shakeAmount;
            cameraPosition.x+=Xaxis;
            cameraPosition.y+=Yaxis;
            mainCam.transform.position=cameraPosition;
        }
    }
    void StopShake()
    {
        CancelInvoke("StartShake");
        mainCam.transform.localPosition=Vector3.zero;
    }
//only for test, will remove later
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CameraShake(0.4f,0.1f);
        }
    }

}
