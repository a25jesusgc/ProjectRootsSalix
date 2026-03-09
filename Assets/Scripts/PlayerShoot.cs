using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        // Instancia el objeto de bala
        GameObject bulletObject = Instantiate(bullet, transform.position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));
        // Se le asigna su movimiento
        Vector2 mov = playerController.GetAimDirection * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;
    }
}
