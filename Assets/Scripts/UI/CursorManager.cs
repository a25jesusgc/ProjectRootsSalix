using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [SerializeField] private Texture2D cursorArrow;
    [SerializeField] private Texture2D cursorCrosshair;
    [SerializeField] private bool startCrosshair;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(startCrosshair)
        {
           SetCursorCrosshair(); 
        }
        else
        {
            SetCursorArrow();
        }
    }

    public void SetCursorArrow()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorCrosshair()
    {
        Cursor.SetCursor(cursorCrosshair, new Vector2(cursorCrosshair.width / 2f, cursorCrosshair.height / 2f), CursorMode.Auto);
    }
}
