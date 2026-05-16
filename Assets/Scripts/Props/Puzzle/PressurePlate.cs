using UnityEngine;

public class PressurePlate : MonoBehaviour, IActivatedProp
{
    [SerializeField] private PuzzleDoor puzzleDoor;
    private Animator anim;

    private bool puzzleCompleted;

    void Start()
    {
        anim = GetComponent<Animator>();
        if(puzzleDoor != null)
        {
            if (PlayerData.GetInstance.WasEventCompleted(puzzleDoor.GetPuzzleID))
            {
                puzzleCompleted = true;
                anim.SetBool("pressed", true);
            }
        }
    }

    // Está activo cuando tiene por lo menos un objeto encima suya
    public bool Activated()
    {
        return pressingAmount > 0;
    }
    
    private int pressingAmount;

    // Cuando algo se pone encima, si es el jugador, un enemigo o un grabbable, se suma a la cantidad de elementos encima de la placa de presión
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(puzzleCompleted) return;
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Grabbable"))
        {
            pressingAmount++;
            puzzleCompleted = puzzleDoor.CheckAllActivated();
            ManageAnim();
        }
    }

    // Cuando algo se pone sale, si es el jugador, un enemigo o un grabbable, se resta de la cantidad de elementos encima de la placa de presión
    void OnTriggerExit2D(Collider2D collision)
    {
        if(puzzleCompleted) return;
        if(puzzleDoor.CheckAllActivated()) {
            puzzleCompleted = true;
            return;
        }
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Grabbable"))
        {
            pressingAmount--;
            puzzleCompleted = puzzleDoor.CheckAllActivated();
            ManageAnim();
        } 
    }

    private void ManageAnim()
    {
        anim.SetBool("pressed", Activated() || puzzleCompleted);
    }
}
