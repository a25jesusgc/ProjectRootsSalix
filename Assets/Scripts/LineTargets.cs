using System.Collections.Generic;
using UnityEngine;

public class LineTargets : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private List<Transform> targets;

    private List<Vector3> positions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = targets.Count;
        positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        positions.Clear();
        foreach (Transform t in targets)
        {
            positions.Add(t.position);
        }
        lineRenderer.SetPositions(positions.ToArray());
    }
}
