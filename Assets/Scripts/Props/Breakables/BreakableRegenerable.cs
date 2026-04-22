using UnityEngine;

public class BreakableRegenerable : BreakableProp
{
    [SerializeField] private Collider2D col;
    [SerializeField] private PuzzleDoor puzzleDoor;
    [SerializeField] private float duration;
    private float timer;
    private bool canRegen = true;

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
