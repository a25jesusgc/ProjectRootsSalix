using UnityEngine;

// Prop que es destruíble y forma parte de un puzzle, de forma que está activo cuando está destruído, pero éste puede regenerarse con el tiempo
public class BreakableRegenerable : BreakableProp
{
    [SerializeField] private Collider2D col;
    [SerializeField] private PuzzleDoor puzzleDoor;
    [SerializeField] private float duration;
    private float timer;
    public bool canRegen = true;

    void Start()
    {
        if(puzzleDoor != null)
        {
            if (PlayerData.GetInstance.WasEventCompleted(puzzleDoor.GetPuzzleID))
            {
                DisableRegeneration();
                if(anim != null) anim.SetTrigger("break");
                isBroken = true;
                col.enabled = false;
            }
        }
    }

    public override void OnBreak()
    {
        timer = 0f;
        col.enabled = false;
        puzzleDoor.CheckAllActivated();
    }

    void Update()
    {
        if (!canRegen || GlobalUtils.pause) return;
        if (timer >= duration)
        {
            if(isBroken) Regenerate();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void Regenerate()
    {
        isBroken = false;
        col.enabled = true;
        if(anim != null) anim.SetTrigger("regenerate");
    }

    public void DisableRegeneration()
    {
        canRegen = false;
    }
}
