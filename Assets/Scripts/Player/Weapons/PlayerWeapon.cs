using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public float shootCooldown;
    public float bulletSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(shootCooldown > 0) shootCooldown -= Time.deltaTime;
    }
    
    public abstract void Shoot();
    public abstract void StopShoot();

}

public static class PlayerWeaponConstants
{
    //public const int CANNON_DAMAGE = 50;
    public const int CANNON_DAMAGE = 5000;
    public const float CANNON_CD = 0.25f;
    public const int FLAMETHROWER_DAMAGE = 20;
    public const float FLAMETHROWER_CD = 0.05f;
    public const float FLAMETHROWER_ANGLE_SPREAD = 8f;
    public const int FLAMETHROWER_SELF_DAMAGE = 12;
    public const int VINE_DAMAGE = 30;
    public const float VINE_CD = 0.25f;
    public const float VINE_DAMAGE_CD = 0.25f;
    public const int VINE_DRAIN_BASE_HEALING = 24;
    public const int VINE_DRAIN_MIN_HEALING = 5;
    public const int VINE_DRAIN_MAX_HEALING = 40;
    public const int SOLAR_RAY_DAMAGE = 120;
    public const float SOLAR_RAY_CD = 0.4f;
    public const int NIGHT_ROOT_DAMAGE = 100;
    public const float NIGHT_ROOT_CD = 0.8f;
}
