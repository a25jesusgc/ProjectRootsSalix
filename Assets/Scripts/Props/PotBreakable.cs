using UnityEngine;

public class PotBreakable : BreakableProp
{
    [SerializeField] private Collider2D col;


    public override void OnBreak()
    {
        Debug.Log("Pot Broken");
        col.enabled = false;
    }
}
