using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivatedProp
{
    [SerializeField] PuzzleDoor puzzleDoor;

    public bool Activated()
    {
        return pressingAmount > 0;
    }
    
    private int pressingAmount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Grabbable"))
        {
            pressingAmount++;
            puzzleDoor.CheckAllActivated();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Grabbable"))
        {
            pressingAmount--;
            puzzleDoor.CheckAllActivated();
        } 
    }
}
