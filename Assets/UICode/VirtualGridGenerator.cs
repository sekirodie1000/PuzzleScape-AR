using UnityEngine;
using Vuforia;

public class VirtualGridGenerator : MonoBehaviour
{
    public ObserverBehaviour observerBehaviour;
    public int rows = 4;
    public int cols = 6;
    public float gridSpacing = 0.065f;

    private Vector3[,] gridPoints;
    private bool isTargetRecognized = false;

    void Start()
    {
        if (observerBehaviour == null)
        {
            Debug.LogError("ObserverBehaviour is not assigned! Please assign it in the Inspector.");
            return;
        }

        if (observerBehaviour is ImageTargetBehaviour imageTargetBehaviour)
        {
            float cardSize = imageTargetBehaviour.GetSize().x;
            gridSpacing = cardSize;
        }

        observerBehaviour.OnTargetStatusChanged += HandleTargetStatusChanged;
    }

    public Vector2Int GetNearestGridPoint(Vector3 markerPosition)
    {
        if (gridPoints == null)
        {
            Debug.LogError("Grid points not generated!");
            return new Vector2Int(-1, -1);
        }

        float minDistance = float.MaxValue;
        Vector2Int nearestPoint = new Vector2Int(-1, -1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float distance = Vector3.Distance(markerPosition, gridPoints[row, col]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPoint = new Vector2Int(row, col);
                }
            }
        }

        return nearestPoint;
    }

    public Vector3[,] GetGridPoints()
    {
        return gridPoints;
    }



    private void HandleTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            if (!isTargetRecognized)
            {
                Debug.Log("AR Target recognized!");
                isTargetRecognized = true;
                GenerateGrid();
            }
        }
        else
        {
            isTargetRecognized = false;
            Debug.Log("AR Target lost!");
        }
    }

    void Update()
    {
        if (isTargetRecognized)
        {
            UpdateGrid();
        }
    }

    void GenerateGrid()
    {
        Debug.Log("Generating grid...");
        gridPoints = new Vector3[rows, cols];
    }

    void UpdateGrid()
    {
        if (gridPoints == null)
        {
            Debug.LogWarning("Grid points not initialized!");
            return;
        }

        Vector3 startPoint = observerBehaviour.transform.position;

        float gridPlaneY = observerBehaviour.transform.position.y;

        Vector3 right = observerBehaviour.transform.right.normalized * gridSpacing;
        Vector3 forward = observerBehaviour.transform.forward.normalized * gridSpacing;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 point = startPoint + row * forward + col * right;
                point.y = gridPlaneY;
                gridPoints[row, col] = point;
            }
        }

        //Debug.Log($"Grid points updated at fixed Y height: {gridPlaneY}");
        GenerateGridLines();
    }


    void GenerateGridLines()
    {
        Transform existingLines = observerBehaviour.transform.Find("GridLines");
        if (existingLines != null)
        {
            Destroy(existingLines.gameObject);
        }

        GameObject gridLinesContainer = new GameObject("GridLines");
        gridLinesContainer.transform.SetParent(observerBehaviour.transform);
        gridLinesContainer.transform.localPosition = Vector3.zero;
        gridLinesContainer.transform.localRotation = Quaternion.identity;

        for (int row = 0; row <= rows; row++)
        {
            GameObject lineObject = new GameObject($"Line_Row_{row}");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            lineRenderer.positionCount = cols + 1;
            lineRenderer.startWidth = 0.001f;
            lineRenderer.endWidth = 0.001f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;

            for (int col = 0; col <= cols; col++)
            {
                Vector3 linePoint = observerBehaviour.transform.position +
                                    observerBehaviour.transform.forward.normalized * row * gridSpacing +
                                    observerBehaviour.transform.right.normalized * col * gridSpacing -
                                    observerBehaviour.transform.forward.normalized * (gridSpacing / 2) -
                                    observerBehaviour.transform.right.normalized * (gridSpacing / 2);
                lineRenderer.SetPosition(col, linePoint);
            }

            lineObject.transform.SetParent(gridLinesContainer.transform);
        }

        for (int col = 0; col <= cols; col++)
        {
            GameObject lineObject = new GameObject($"Line_Column_{col}");
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            lineRenderer.positionCount = rows + 1;
            lineRenderer.startWidth = 0.001f;
            lineRenderer.endWidth = 0.001f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;

            for (int row = 0; row <= rows; row++)
            {
                Vector3 linePoint = observerBehaviour.transform.position +
                                    observerBehaviour.transform.forward.normalized * row * gridSpacing +
                                    observerBehaviour.transform.right.normalized * col * gridSpacing -
                                    observerBehaviour.transform.forward.normalized * (gridSpacing / 2) -
                                    observerBehaviour.transform.right.normalized * (gridSpacing / 2);
                lineRenderer.SetPosition(row, linePoint);
            }

            lineObject.transform.SetParent(gridLinesContainer.transform);
        }
    }

    void GenerateGridVisuals()
    {
        Transform existingGrid = observerBehaviour.transform.Find("GridCubes");
        if (existingGrid != null)
        {
            Destroy(existingGrid.gameObject);
        }

        GameObject gridCubesContainer = new GameObject("GridCubes");
        gridCubesContainer.transform.SetParent(observerBehaviour.transform);
        gridCubesContainer.transform.localPosition = Vector3.zero;
        gridCubesContainer.transform.localRotation = Quaternion.identity;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject gridPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);

                gridPoint.transform.position = gridPoints[row, col];

                gridPoint.transform.localScale = Vector3.one * gridSpacing * 0.8f;

                gridPoint.name = $"GridPoint_{row}_{col}";

                gridPoint.transform.SetParent(gridCubesContainer.transform);

                Renderer renderer = gridPoint.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = new Material(Shader.Find("Standard"));
                    renderer.material.color = Color.green;
                }
            }
        }
    }

}