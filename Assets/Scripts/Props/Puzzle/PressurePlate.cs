using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivatedProp
{
    [SerializeField] private PuzzleDoor puzzleDoor;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

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
            ManageAnim();
            puzzleDoor.CheckAllActivated();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Grabbable"))
        {
            pressingAmount--;
            ManageAnim();
            puzzleDoor.CheckAllActivated();
        } 
    }

    private void ManageAnim()
    {
        anim.SetBool("pressed", Activated());
    }
}
