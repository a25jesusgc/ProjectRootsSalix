using UnityEngine;
using Unity.Cinemachine;

public class DoorTeleport : MonoBehaviour
{
    public Transform destino;
    public Collider2D confinerInterior;

    private CinemachineConfiner2D confiner;
    private CinemachineCamera camara;

    private void Start()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
        camara = FindObjectOfType<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destino.position;

            // cambiar confiner
            confiner.BoundingShape2D = confinerInterior;
            confiner.InvalidateCache();

            // zoom interior
            camara.Lens.OrthographicSize = 5f;
        }
    }
}