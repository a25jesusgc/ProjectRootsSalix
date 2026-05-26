using UnityEngine;

public class PlayEnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject attack;

    public void ActivateAttack()
    {
        attack.SetActive(true);
    }

    public void StopAttack()
    {
        attack.SetActive(false);
    }
}
