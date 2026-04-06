using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public float shootCooldown;
    public float bulletSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shootCooldown > 0) shootCooldown -= Time.deltaTime;
    }
    
    public abstract void Shoot();

}
