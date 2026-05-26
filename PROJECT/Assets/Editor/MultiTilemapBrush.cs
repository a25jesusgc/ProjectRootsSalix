using UnityEngine;
using UnityEditor;

using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TreeBrush", menuName = "Brushes/Tree Brush")]
[CustomGridBrush(false, true, false, "Tree Brush")]
public class TreeBrush : GridBrushBase
{
    public TileBase baseLeft;
    public TileBase baseRight;

    public TileBase trunkLeft;
    public TileBase trunkRight;

    public TileBase topLeft;
    public TileBase topRight;

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget == null)
            return;

        // Tilemap donde estás pintando (por ejemplo BackgroundDecoration)
        Tilemap baseMap = brushTarget.GetComponent<Tilemap>();
        if (baseMap == null)
            return;

        Transform parent = brushTarget.transform.parent;
        if (parent == null)
            return;
        
        baseMap = parent.Find("BackgroundDecoration")?.GetComponent<Tilemap>();
        // OJO: los nombres deben coincidir con tus GameObjects
        Tilemap trunkMap = parent.Find("Wall")?.GetComponent<Tilemap>();
        Tilemap topMap   = parent.Find("FrontDecoration")?.GetComponent<Tilemap>();

        if (trunkMap == null || topMap == null)
        {
            Debug.LogWarning("No se encontraron los Tilemaps 'Wall' o 'FrontDecoration' como hijos del mismo Grid.");
            return;
        }

        // BASE (BackgroundDecoration)
        baseMap.SetTile(position, baseLeft);
        baseMap.SetTile(position + Vector3Int.right, baseRight);

        // TRONCO (Wall)
        trunkMap.SetTile(position + Vector3Int.up, trunkLeft);
        trunkMap.SetTile(position + Vector3Int.up + Vector3Int.right, trunkRight);

        // COPA (FrontDecoration)
        topMap.SetTile(position + Vector3Int.up * 2, topLeft);
        topMap.SetTile(position + Vector3Int.up * 2 + Vector3Int.right, topRight);
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
{
    if (brushTarget == null)
        return;

    Tilemap map = brushTarget.GetComponent<Tilemap>();
    if (map == null)
        return;

    map.SetTile(position, null);
}

}
