using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

enum CameraType
{
    isometric,
    perspective
}
public class CameraController : MonoBehaviour
{
    [SerializeField] CameraType currentCameraType = CameraType.perspective;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineCameraOffset cameraOffset;
    [Header("Isometric Settings")]
    [SerializeField] float minDistIso = 1.7f;
    [SerializeField] float maxDistIso = 3.5f;
    [SerializeField] float newDistIso;
    [SerializeField] float sensitivityZoomingIso = 0.1f;
    [SerializeField] float startDistIso = 3f;

    [Header("Perspective Settings")]
    [SerializeField] float minDistPer = -155f;
    [SerializeField] float maxDistPer = -55f;
    [SerializeField] float newDistPer;
    [SerializeField] float sensitivityZoomingPer = 15f;
    [SerializeField] float startDistPer = -90f;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraOffset = GetComponent<CinemachineCameraOffset>();
        //virtualCamera.m_Lens.OrthographicSize = startDistIso;
        //newDistIso = virtualCamera.m_Lens.OrthographicSize;

        cameraOffset.m_Offset.z = startDistPer;
        newDistPer = cameraOffset.m_Offset.z;
    }
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && currentCameraType == CameraType.isometric)
        {
            newDistIso -= Input.GetAxis("Mouse ScrollWheel") * sensitivityZoomingIso;
            newDistIso = Mathf.Clamp(newDistIso, minDistIso, maxDistIso);
            virtualCamera.m_Lens.OrthographicSize = newDistIso;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && currentCameraType == CameraType.perspective)
       {
            newDistPer += Input.GetAxis("Mouse ScrollWheel") * sensitivityZoomingPer;
            newDistPer = Mathf.Clamp(newDistPer, minDistPer, maxDistPer);
            cameraOffset.m_Offset.z = newDistPer;
        }
    }
    
}
