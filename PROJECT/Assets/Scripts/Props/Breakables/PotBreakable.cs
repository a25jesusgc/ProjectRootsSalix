using UnityEngine;

public class PotBreakable : BreakableProp
{
    [SerializeField] private Collider2D col;
    [SerializeField] private DropSpawner dropSpawner;


    public override void OnBreak()
    {
        col.enabled = false;
        dropSpawner.SpawnDrops();
    }
}
