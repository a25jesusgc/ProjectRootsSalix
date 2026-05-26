using UnityEngine;

// Prop destruible que es parte de un puzzle, y por ende al ser destruído, comprueba si se completó el puzle
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
