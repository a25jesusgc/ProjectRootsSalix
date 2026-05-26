using System.Linq;
using UnityEngine;

public class PlayerCannon : PlayerWeapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioSource shootSound;

    [SerializeField] private FertilizerSelector fertilizerSelector;
    [SerializeField] private Fertilizer[] fertilizers;
    [SerializeField] private PlayerFertilizerSelection fertilizerSelection;
    private PlayerFertilizer chosenFertilizer;

    public override void Shoot()
    {
        // Si está recargándose, no dispara
        if(shootCooldown > 0) return;

        Vector3 position = transform.position + new Vector3(playerController.GetAimDirection.x, playerController.GetAimDirection.y);

        // Instancia el objeto de bala
        GameObject bulletObject = Instantiate(chosenFertilizer != null ? GetFertilizer(chosenFertilizer).GetProjectile : bullet, position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));
        // Se le asigna su movimiento
        Vector2 mov = playerController.GetAimDirection * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;
        bulletObject.GetComponent<BulletHit>().damage = PlayerWeaponConstants.CANNON_DAMAGE;

        shootSound.Play();

        if (chosenFertilizer != null)
        {
            bool ranOutOfFertilizer = PlayerData.GetInstance.UseFertilizer(chosenFertilizer);
            if(ranOutOfFertilizer)
            {
                fertilizerSelector.SetEmptyFertilizer();
            }
            else
            {
                fertilizerSelection.UpdateSelectedFertilizerIconAndAmount(GetFertilizer(chosenFertilizer).GetIcon, chosenFertilizer.GetFertilizerAmount);
            }
        }

        // Tras disparar necesita recargarse
        shootCooldown = PlayerWeaponConstants.CANNON_CD;
    }

    public override void StopShoot()
    {
        
    }

    public void SetCurrentFertilizer(PlayerFertilizer playerFertilizer)
    {
        chosenFertilizer = playerFertilizer;
        if(fertilizerSelection != null)
        {
            if (playerFertilizer != null)
            {
                fertilizerSelection.UpdateSelectedFertilizerIconAndAmount(GetFertilizer(chosenFertilizer).GetIcon, chosenFertilizer.GetFertilizerAmount);
            }
            else
            {
                fertilizerSelection.UpdateSelectedFertilizerIconAndAmount(null, 0);
            }
        } 
    }

    private Fertilizer GetFertilizer(PlayerFertilizer playerFertilizer)
    {
        return fertilizers.First((f) => f.GetFertilizerType == playerFertilizer.GetFertilizerType);
    }
}
