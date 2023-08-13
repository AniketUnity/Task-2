using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<GameObject> circles = new List<GameObject>();

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(0, GetMousePosition());
        }
        else if (Input.GetMouseButton(0))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, GetMousePosition());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;
            CheckForIntersectingCircles();
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }

    private void CheckForIntersectingCircles()
    {
        Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);

        foreach (GameObject circle in circles)
        {
            CircleCollider2D collider = circle.GetComponent<CircleCollider2D>();
            foreach (Vector3 linePoint in linePositions)
            {
                if (collider.OverlapPoint(linePoint))
                {
                    Destroy(circle);
                    break;
                }
            }
        }
    }
}
