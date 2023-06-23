using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SunAngle
{
    public float angleX;
    public float angleY;
    public float angleZ;
}
public class SunController : MonoBehaviour
{
    [SerializeField] SunAngle sunStartAngle;
    [SerializeField] SunAngle sunAngle;
    [SerializeField] SunAngle sunCurrentAngle;
    [SerializeField] float sunSpeed = 0;

    [SerializeField] bool X,Y, Z;
    private void Start()
    {
        sunCurrentAngle.angleX = 0;
        sunCurrentAngle.angleY = 0;
        sunCurrentAngle.angleZ = 0;
        transform.localRotation = Quaternion.Euler(sunStartAngle.angleX, sunStartAngle.angleY, sunStartAngle.angleZ);
    }

    public void FixedUpdate()
    {
        if (X)
        {
            sunCurrentAngle.angleX = Mathf.PingPong(Time.time * sunSpeed, sunAngle.angleX);
        }
        if (Y)
        {
            sunCurrentAngle.angleY = Mathf.PingPong(Time.time * sunSpeed, sunAngle.angleY);
        }
        if (Z)
        {
            sunCurrentAngle.angleZ = Mathf.PingPong(Time.time * sunSpeed, sunAngle.angleZ);
        }

        transform.localRotation = Quaternion.Euler(sunCurrentAngle.angleX + sunAngle.angleX, sunCurrentAngle.angleY + sunAngle.angleY, sunCurrentAngle.angleZ+ +sunAngle.angleZ);   
    }
}




