using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            CameraController.instance.SetConfiner(GetComponent<Collider2D>());
        }
    }
}
