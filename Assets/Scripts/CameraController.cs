using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner;
    [SerializeField] private CinemachinePositionComposer cinemachinePositionCompposer;

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

    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(CameraShake(intensity / 100f, duration));
    }

    private IEnumerator CameraShake(float intensity, float duration)
    {
        float timer = duration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            cinemachinePositionCompposer.Composition.ScreenPosition = new Vector2(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));

            yield return null;
        }
        cinemachinePositionCompposer.Composition.ScreenPosition = Vector2.zero;
    }

}
