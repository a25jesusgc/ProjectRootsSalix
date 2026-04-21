using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;

    [SerializeField] private Transform player;

    void Awake()
    {
        instance = this;
    }

    public void SetTrackingTarget(Transform target)
    {
        cinemachineCamera.Follow = target;
    }

    public void ResetTrackingTarget()
    {
        cinemachineCamera.Follow = player;
    }

    public void SetConfiner(Collider2D bounding)
    {
        cinemachineConfiner.BoundingShape2D = bounding;
    }

}
