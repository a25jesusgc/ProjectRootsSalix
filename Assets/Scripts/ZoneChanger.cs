using UnityEngine;

public class ZoneChanger : MonoBehaviour
{
    [SerializeField] private string unload;
    [SerializeField] private string load;
    [SerializeField] private Transform target;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ZoneLoader.instance.ChangeZone(unload, load, target.position);
        }
    }
}
