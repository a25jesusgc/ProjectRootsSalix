using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
