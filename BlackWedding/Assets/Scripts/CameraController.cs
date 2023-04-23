using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float minDist = 1.7f;
    [SerializeField] float maxDist = 3.5f;
    [SerializeField] float newDist;
    [SerializeField] float sensitivityZooming = 0.1f;
    [SerializeField] float startDist = 3f;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.OrthographicSize = startDist;
        newDist = virtualCamera.m_Lens.OrthographicSize;
    }
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            newDist -= Input.GetAxis("Mouse ScrollWheel") * sensitivityZooming;
            newDist = Mathf.Clamp(newDist, minDist, maxDist);
            virtualCamera.m_Lens.OrthographicSize = newDist;
        }
    }
    
}
