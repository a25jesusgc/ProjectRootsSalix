using UnityEngine;

public class PotBreakable : BreakableProp
{
    [SerializeField] private Collider2D col;


    public override void OnBreak()
    {
        col.enabled = false;
    }
}
