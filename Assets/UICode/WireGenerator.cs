using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class WireGenerator : MonoBehaviour
{
    public GameObject wirePrefab;
    public Material grayMaterial;
    public Material yellowMaterial;
    public VirtualGridGenerator gridGenerator;
    private List<GameObject> wires = new List<GameObject>();

    public GameObject normalLampPrefab;
    public GameObject glowingLampPrefab;
    private Dictionary<int, GameObject> normalLampInstances = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> glowingLampInstances = new Dictionary<int, GameObject>();

    public GameObject batteryNormalPrefab;
    public GameObject batterySparkingPrefab;
    private GameObject batteryNormalInstance;
    private GameObject batterySparkingInstance;

    public GameObject switchOnPrefab;
    public GameObject switchOffPrefab;
    private GameObject switchOnInstance;
    private GameObject switchOffInstance;

    public int rows;
    public int cols;

    public enum CircuitType { Series, Parallel }

    public CircuitType currentCircuitType = CircuitType.Series;




    private List<Tuple<Vector2Int, Vector2Int>> GenerateCircuitWireSegments(List<Vector2Int> componentPositions)
    {
        int maxRow = 0;
        foreach (var pos in componentPositions)
        {
            if (pos.x > maxRow)
            {
                maxRow = pos.x;
            }
        }

        List<Tuple<Vector2Int, Vector2Int>> wireSegments = new List<Tuple<Vector2Int, Vector2Int>>();

        wireSegments.Add(Tuple.Create(new Vector2Int(0, 0), new Vector2Int(0, cols - 1)));
        wireSegments.Add(Tuple.Create(new Vector2Int(0, cols - 1), new Vector2Int(maxRow, cols - 1)));
        wireSegments.Add(Tuple.Create(new Vector2Int(maxRow, cols - 1), new Vector2Int(maxRow, 0)));
        wireSegments.Add(Tuple.Create(new Vector2Int(maxRow, 0), new Vector2Int(0, 0)));

        foreach (var pos in componentPositions)
        {
            if (pos.x < maxRow && pos.y != 0 && pos.y != cols - 1)
            {
                wireSegments.Add(Tuple.Create(new Vector2Int(pos.x, 0), new Vector2Int(pos.x, cols - 1)));
            }
        }

        return wireSegments;
    }

    private void CreateWire(Vector3 start, Vector3 end)
    {
        float fixedY = gridGenerator.GetGridPoints()[0, 0].y;
        start.y = fixedY;
        end.y = fixedY;

        GameObject wire = Instantiate(wirePrefab);
        wire.transform.position = (start + end) / 2;
        wire.transform.localScale = new Vector3(0.001f, 0.001f, Vector3.Distance(start, end));
        wire.transform.LookAt(end);
        wire.GetComponent<Renderer>().material = grayMaterial;
        wires.Add(wire);
    }


    public void ClearWires()
    {
        foreach (var wire in wires)
        {
            Destroy(wire);
        }
        wires.Clear();
    }




    public void UpdateCircuit(List<Vector2Int> componentPositions, Vector2Int batteryPosition, Vector2Int switchPosition, int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        ClearWires();

        List<Vector2Int> allComponents = new List<Vector2Int>(componentPositions);
        if (batteryPosition.x >= 0 && batteryPosition.y >= 0)
        {
            allComponents.Add(batteryPosition);
        }
        if (switchPosition.x >= 0 && switchPosition.y >= 0)
        {
            allComponents.Add(switchPosition);
        }

        if (allComponents.Count == 0)
        {
            return;
        }

        Vector3[,] gridPoints = gridGenerator.GetGridPoints();

        if (gridPoints == null)
        {
            Debug.LogError("Grid points not initialized in GridGenerator!");
            return;
        }

        var wireSegments = GenerateCircuitWireSegments(allComponents);

        foreach (var segment in wireSegments)
        {
            Vector2Int start = segment.Item1;
            Vector2Int end = segment.Item2;

            if (start.x >= 0 && start.x < rows && start.y >= 0 && start.y < cols &&
                end.x >= 0 && end.x < rows && end.y >= 0 && end.y < cols)
            {
                CreateWire(gridPoints[start.x, start.y], gridPoints[end.x, end.y]);
            }
            else
            {
                Debug.LogError($"Wire segment out of bounds: {start} -> {end}");
            }
        }
    }

    public void CheckCircuitCompletion(List<Vector2Int> lampPositions, Vector2Int batteryPosition, Vector2Int switchPosition)
    {
        bool isCompleteCircuit = false;

        if (batteryPosition.x >= 0 && batteryPosition.y >= 0 &&
            switchPosition.x >= 0 && switchPosition.y >= 0 &&
            lampPositions.Count >= 2)
        {

            int wireSegmentCount = wires.Count;

            if (currentCircuitType == CircuitType.Series)
            {
                if (wireSegmentCount <= 4)
                {
                    isCompleteCircuit = true;
                }
            }
            else if (currentCircuitType == CircuitType.Parallel)
            {
                if (wireSegmentCount >= 4)
                {
                    if (lampPositions[0].x != lampPositions[1].x)
                    {
                        bool switchNotInLampRow = (switchPosition.x != lampPositions[0].x) && (switchPosition.x != lampPositions[1].x);
                        if (switchNotInLampRow)
                        {
                            isCompleteCircuit = true;
                        }
                    }
                }
            }
        }

        if (isCompleteCircuit)
        {
            Debug.Log("Game success! Circuit is complete.");
            SetWireMaterial(yellowMaterial);

            foreach (var lampID in normalLampInstances.Keys)
            {
                if (glowingLampInstances.ContainsKey(lampID))
                {
                    normalLampInstances[lampID].SetActive(false);
                    glowingLampInstances[lampID].SetActive(true);
                }
            }

            if (switchOnInstance != null)
                switchOnInstance.SetActive(true);
            if (switchOffInstance != null)
                switchOffInstance.SetActive(false);
        }
        else
        {
            Debug.Log("Circuit incomplete.");
            SetWireMaterial(grayMaterial);

            foreach (var lampID in normalLampInstances.Keys)
            {
                if (glowingLampInstances.ContainsKey(lampID))
                {
                    normalLampInstances[lampID].SetActive(true);
                    glowingLampInstances[lampID].SetActive(false);
                }
            }

            if (switchOnInstance != null)
                switchOnInstance.SetActive(false);
            if (switchOffInstance != null)
                switchOffInstance.SetActive(true);
        }

        if (switchPosition.x >= 0 && switchPosition.y >= 0)
        {
            if (batteryNormalInstance != null)
                batteryNormalInstance.SetActive(true);
            if (batterySparkingInstance != null)
                batterySparkingInstance.SetActive(false);
        }
        else
        {
            if (batteryNormalInstance != null)
                batteryNormalInstance.SetActive(false);
            if (batterySparkingInstance != null)
                batterySparkingInstance.SetActive(true);
        }
    }





    private void SetWireMaterial(Material material)
    {
        foreach (var wire in wires)
        {
            wire.GetComponent<Renderer>().material = material;
        }
    }



    public void UpdateLampModel(Vector2Int lampPosition, int lampID)
    {
        if (lampPosition.x >= 0 && lampPosition.y >= 0)
        {
            GameObject normalLampInstance;
            GameObject glowingLampInstance;

            // Instantiate normal lamp if not already instantiated
            if (!normalLampInstances.TryGetValue(lampID, out normalLampInstance))
            {
                normalLampInstance = Instantiate(normalLampPrefab);
                normalLampInstances[lampID] = normalLampInstance;
            }

            // Instantiate glowing lamp if not already instantiated
            if (!glowingLampInstances.TryGetValue(lampID, out glowingLampInstance))
            {
                glowingLampInstance = Instantiate(glowingLampPrefab);
                glowingLampInstances[lampID] = glowingLampInstance;
            }

            // Set position and scale
            Vector3 lampWorldPosition = gridGenerator.GetGridPoints()[lampPosition.x, lampPosition.y];
            float scaleFactor = gridGenerator.gridSpacing * 0.25f;

            normalLampInstance.transform.position = lampWorldPosition;
            normalLampInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            glowingLampInstance.transform.position = lampWorldPosition;
            glowingLampInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else
        {
            // Remove lamp instances if position is invalid
            if (normalLampInstances.ContainsKey(lampID))
            {
                Destroy(normalLampInstances[lampID]);
                normalLampInstances.Remove(lampID);
            }
            if (glowingLampInstances.ContainsKey(lampID))
            {
                Destroy(glowingLampInstances[lampID]);
                glowingLampInstances.Remove(lampID);
            }
        }
    }





    public void UpdateSwitchModel(Vector2Int switchPosition)
    {
        if (switchPosition.x >= 0 && switchPosition.y >= 0)
        {
            if (switchOffInstance == null)
            {
                switchOffInstance = Instantiate(switchOffPrefab);
            }

            if (switchOnInstance == null)
            {
                switchOnInstance = Instantiate(switchOnPrefab);
            }

            Vector3 switchWorldPosition = gridGenerator.GetGridPoints()[switchPosition.x, switchPosition.y];
            float scaleFactor = gridGenerator.gridSpacing * 0.5f;

            switchOffInstance.transform.position = switchWorldPosition;
            switchOffInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            switchOnInstance.transform.position = switchWorldPosition;
            switchOnInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            switchOffInstance.SetActive(true);
            switchOnInstance.SetActive(false);
        }
        else
        {
            if (switchOffInstance != null)
            {
                Destroy(switchOffInstance);
                switchOffInstance = null;
            }

            if (switchOnInstance != null)
            {
                Destroy(switchOnInstance);
                switchOnInstance = null;
            }
        }
    }

    public void UpdateBatteryModel(Vector2Int batteryPosition, Vector2Int switchPosition)
    {
        if (batteryPosition.x >= 0 && batteryPosition.y >= 0)
        {
            if (batteryNormalInstance == null)
            {
                batteryNormalInstance = Instantiate(batteryNormalPrefab);
            }

            if (batterySparkingInstance == null)
            {
                batterySparkingInstance = Instantiate(batterySparkingPrefab);
            }

            Vector3 batteryWorldPosition = gridGenerator.GetGridPoints()[batteryPosition.x, batteryPosition.y];
            float scaleFactor = gridGenerator.gridSpacing * 0.25f;

            batteryNormalInstance.transform.position = batteryWorldPosition;
            batteryNormalInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            batterySparkingInstance.transform.position = batteryWorldPosition;
            batterySparkingInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        }
        else
        {
            if (batteryNormalInstance != null)
            {
                Destroy(batteryNormalInstance);
                batteryNormalInstance = null;
            }

            if (batterySparkingInstance != null)
            {
                Destroy(batterySparkingInstance);
                batterySparkingInstance = null;
            }
        }
    }





}
