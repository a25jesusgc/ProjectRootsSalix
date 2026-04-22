using UnityEngine;

public class BreakablePuzzle : BreakableProp
{
    [SerializeField] private Collider2D col;
    [SerializeField] private PuzzleDoor puzzleDoor;

    public override void OnBreak()
    {
        col.enabled = false;
        puzzleDoor.CheckAllActivated();
    }
}
