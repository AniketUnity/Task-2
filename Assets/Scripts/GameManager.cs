using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject lineRendererObject;

    private LineRenderer lineRenderer;
    private Vector2 lineStart;
    private Vector2 lineEnd;
    private bool drawing = false;

    private List<GameObject> circles = new List<GameObject>();

    void Start()
    {
        lineRenderer = lineRendererObject.GetComponent<LineRenderer>();
        CreateCircles(Random.Range(5, 11));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
            CheckCollisionsAndDestroy();
            lineRenderer.positionCount = 0;
        }

        if (drawing)
        {
            Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, lineStart);
            lineRenderer.SetPosition(1, currentPos);
        }
    }

    void StartDrawing()
    {
        drawing = true;
        lineStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void StopDrawing()
    {
        drawing = false;
    }

    void CreateCircles(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-8f, 8f);
            float y = Random.Range(-4f, 4f);
            Vector3 spawnPosition = new Vector3(x, y, 0f);
            GameObject circle = Instantiate(circlePrefab, spawnPosition, Quaternion.identity);
            circles.Add(circle);
        }
    }

    void CheckCollisionsAndDestroy()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject circle in circles)
        {
            CircleCollider2D collider = circle.GetComponent<CircleCollider2D>();
            if (IsLineIntersectingCircle(collider))
            {
                toRemove.Add(circle);
            }
        }

        foreach (GameObject circle in toRemove)
        {
            circles.Remove(circle);
            Destroy(circle);
        }
    }


    bool IsLineIntersectingCircle(CircleCollider2D circleCollider)
    {
        Vector3[] linePositions = new Vector3[2];
        lineRenderer.GetPositions(linePositions);

        Vector2 circleCenter = circleCollider.bounds.center;
        float circleRadius = circleCollider.radius;

        for (int i = 0; i < linePositions.Length - 1; i++)
        {
            Vector2 startPoint = linePositions[i];
            Vector2 endPoint = linePositions[i + 1];

            bool intersects = LineIntersectsCircle(startPoint, endPoint, circleCenter, circleRadius);

            if (intersects)
            {
                return true;
            }
        }

        return false;
    }

    bool LineIntersectsCircle(Vector2 start, Vector2 end, Vector2 circleCenter, float circleRadius)
    {
        float sqrRadius = circleRadius * circleRadius;

        Vector2 lineDir = (end - start).normalized;
        Vector2 closestPoint = start + Vector2.Dot(circleCenter - start, lineDir) * lineDir;

        if (Vector2.SqrMagnitude(circleCenter - closestPoint) <= sqrRadius)
        {
            if (Vector2.Dot(end - start, closestPoint - start) >= 0 && Vector2.Dot(start - end, closestPoint - end) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    public void Restart()
    {
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();
        CreateCircles(Random.Range(5, 11));
    }
}
