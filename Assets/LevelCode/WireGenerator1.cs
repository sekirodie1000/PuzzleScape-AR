using System;
using System.Collections.Generic;
using UnityEngine;

public class WireGenerator1 : MonoBehaviour
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

    // 电流表 (第一关用)
    public GameObject ammeterNormalPrefab;
    public GameObject ammeterReadingPrefab;
    private GameObject ammeterInstance;

    // 电压表 (第二关用)
    public GameObject voltmeterNormalPrefab;
    public GameObject voltmeterReadingPrefab;
    private GameObject voltmeterInstance;

    // 用于区分关卡
    public bool isLevel2 = false;

    // 第一关初始条件
    private List<Vector2Int> initialLampPositions_Level1 = new List<Vector2Int> { new Vector2Int(3, 1), new Vector2Int(1, 5) };
    private Vector2Int initialSwitchPosition_Level1 = new Vector2Int(2, 1);
    private Vector2Int initialBatteryPosition_Level1 = new Vector2Int(0, 1);

    // 第二关初始条件（坏灯泡在(3,1)位置，好灯泡在(3,4)位置）
    private List<Vector2Int> initialLampPositions_Level2 = new List<Vector2Int> { new Vector2Int(3, 1), new Vector2Int(3, 4) };
    private Vector2Int initialSwitchPosition_Level2 = new Vector2Int(0, 2);
    private Vector2Int initialBatteryPosition_Level2 = new Vector2Int(0, 1);

    private bool initialCheckDone = false;
    private bool puzzleModeStarted = false;

    public PuzzleUIManager puzzleUIManager;

    public int rows;
    public int cols;

    private List<Tuple<Vector2Int, Vector2Int>> GenerateCircuitWireSegments(List<Vector2Int> componentPositions, Vector2Int switchPosition)
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

        var wireSegments = GenerateCircuitWireSegments(allComponents, switchPosition);

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
        ARMarkerDetector1 detector = FindObjectOfType<ARMarkerDetector1>();

        if (!isLevel2)
        {
            // 第一关逻辑
            if (!initialCheckDone && !puzzleModeStarted)
            {
                HashSet<Vector2Int> initSet = new HashSet<Vector2Int>(initialLampPositions_Level1);
                HashSet<Vector2Int> currentSet = new HashSet<Vector2Int>(lampPositions);

                bool initialCorrect = initSet.SetEquals(currentSet);

                if (switchPosition != initialSwitchPosition_Level1) initialCorrect = false;
                if (batteryPosition != initialBatteryPosition_Level1) initialCorrect = false;

                if (initialCorrect)
                {
                    initialCheckDone = true;
                    puzzleUIManager.ShowPopup("Initial Layout Correct", showBegin: true);
                }
                else
                {
                    puzzleUIManager.ShowPopup("Initial Layout Errors", showBegin: false);
                }
                return;
            }

            if (initialCheckDone && puzzleModeStarted)
            {
                int changes = 0;
                if (switchPosition != initialSwitchPosition_Level1) changes++;
                if (batteryPosition != initialBatteryPosition_Level1) changes++;

                bool onBoundary = IsSwitchOnBoundary(switchPosition);

                bool isCompleteCircuit = (changes == 1 && onBoundary);

                UpdateLampStates(lampPositions, isCompleteCircuit);

                if (batteryNormalInstance != null) batteryNormalInstance.SetActive(true);
                if (batterySparkingInstance != null) batterySparkingInstance.SetActive(false);
                if (switchOnInstance != null) switchOnInstance.SetActive(true);

                if (isCompleteCircuit)
                {
                    SetWireMaterial(yellowMaterial);
                    puzzleUIManager.ShowPopup("Puzzle Solve", showBegin: false, nextScene: "NextSceneName");

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
                    SetWireMaterial(grayMaterial);
                    puzzleUIManager.ShowPopup("Failure!", showBegin: false);

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
            }
        }
        else
        {
            // 第二关逻辑
            if (!initialCheckDone && !puzzleModeStarted)
            {
                HashSet<Vector2Int> initSet = new HashSet<Vector2Int>(initialLampPositions_Level2);
                HashSet<Vector2Int> currentSet = new HashSet<Vector2Int>(lampPositions);

                bool initialCorrect = initSet.SetEquals(currentSet);
                if (switchPosition != initialSwitchPosition_Level2) initialCorrect = false;
                if (batteryPosition != initialBatteryPosition_Level2) initialCorrect = false;

                if (initialCorrect)
                {
                    initialCheckDone = true;
                    puzzleUIManager.ShowPopup("Initial Layout Correct (Level 2)", showBegin: true);
                }
                else
                {
                    puzzleUIManager.ShowPopup("Initial Layout Errors (Level 2)", showBegin: false);
                }
                return;
            }

            if (initialCheckDone && puzzleModeStarted)
            {
                bool batteryCorrect = (batteryPosition == initialBatteryPosition_Level2);
                bool switchCorrect = (switchPosition == initialSwitchPosition_Level2);

                HashSet<Vector2Int> targetLampSet = new HashSet<Vector2Int>(initialLampPositions_Level2);
                HashSet<Vector2Int> currentLampSet = new HashSet<Vector2Int>(lampPositions);
                bool lampLayoutCorrect = targetLampSet.SetEquals(currentLampSet);

                // 核心检查： (3,1)位置的灯是否为好灯泡
                int lampID_3_1 = detector.GetLampInstanceIDAtPosition(new Vector2Int(3, 1));
                bool is3_1LampGood = detector.IsLampGood(lampID_3_1);

                bool allCorrect = batteryCorrect && switchCorrect && lampLayoutCorrect && is3_1LampGood;

                if (allCorrect)
                {
                    SetWireMaterial(yellowMaterial);
                    puzzleUIManager.ShowPopup("Level 2 Puzzle Solved!", showBegin: false, nextScene: "NextSceneName");

                    foreach (var lampID in normalLampInstances.Keys)
                    {
                        if (glowingLampInstances.ContainsKey(lampID))
                        {
                            normalLampInstances[lampID].SetActive(false);
                            glowingLampInstances[lampID].SetActive(true);
                        }
                    }
                }
                else
                {
                    SetWireMaterial(grayMaterial);
                    puzzleUIManager.ShowPopup("Level 2 Failure!", showBegin: false);

                    foreach (var lampID in normalLampInstances.Keys)
                    {
                        if (glowingLampInstances.ContainsKey(lampID))
                        {
                            normalLampInstances[lampID].SetActive(false);
                            glowingLampInstances[lampID].SetActive(true);
                        }
                    }
                }
            }
        }
    }

    private bool IsSwitchOnBoundary(Vector2Int sp)
    {
        if ((sp.x == 0 && sp.y >= 0 && sp.y <= 5) ||
            (sp.x == 3 && sp.y >= 0 && sp.y <= 5) ||
            (sp.y == 0 && sp.x >= 0 && sp.x <= 3) ||
            (sp.y == 5 && sp.x >= 0 && sp.x <= 3))
        {
            return true;
        }
        return false;
    }

    private void UpdateLampStates(List<Vector2Int> lampPositions, bool isCompleteCircuit)
    {
        foreach (var kvp in normalLampInstances)
        {
            var lampID = kvp.Key;
            var normalLamp = kvp.Value;
            var glowingLamp = glowingLampInstances.ContainsKey(lampID) ? glowingLampInstances[lampID] : null;

            if (isCompleteCircuit)
            {
                normalLamp.SetActive(false);
                if (glowingLamp != null)
                    glowingLamp.SetActive(true);
            }
            else
            {
                normalLamp.SetActive(true);
                if (glowingLamp != null)
                    glowingLamp.SetActive(false);
            }
        }
    }

    public void UpdateLampModel(Vector2Int lampPosition, int lampID)
    {
        if (lampPosition.x >= 0 && lampPosition.y >= 0)
        {
            GameObject normalLampInstance;
            GameObject glowingLampInstance;

            if (!normalLampInstances.TryGetValue(lampID, out normalLampInstance))
            {
                normalLampInstance = Instantiate(normalLampPrefab);
                normalLampInstances[lampID] = normalLampInstance;
            }

            if (!glowingLampInstances.TryGetValue(lampID, out glowingLampInstance))
            {
                glowingLampInstance = Instantiate(glowingLampPrefab);
                glowingLampInstances[lampID] = glowingLampInstance;
            }

            Vector3 lampWorldPosition = gridGenerator.GetGridPoints()[lampPosition.x, lampPosition.y];
            float scaleFactor = gridGenerator.gridSpacing * 0.25f;

            normalLampInstance.transform.position = lampWorldPosition;
            normalLampInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            glowingLampInstance.transform.position = lampWorldPosition;
            glowingLampInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            normalLampInstance.SetActive(true);
            glowingLampInstance.SetActive(false);
        }
        else
        {
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
        if (switchOffInstance != null)
        {
            Destroy(switchOffInstance);
            switchOffInstance = null;
        }

        if (switchOnInstance == null)
        {
            switchOnInstance = Instantiate(switchOnPrefab);
        }

        if (switchPosition.x >= 0 && switchPosition.y >= 0)
        {
            Vector3 switchWorldPosition = gridGenerator.GetGridPoints()[switchPosition.x, switchPosition.y];
            float scaleFactor = gridGenerator.gridSpacing * 0.5f;

            switchOnInstance.transform.position = switchWorldPosition;
            switchOnInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            switchOnInstance.SetActive(true);
        }
        else
        {
            switchOnInstance.SetActive(false);
        }
    }

    public void UpdateBatteryModel(Vector2Int batteryPosition, Vector2Int switchPosition)
    {
        if (batterySparkingInstance != null)
        {
            Destroy(batterySparkingInstance);
            batterySparkingInstance = null;
        }

        if (batteryNormalInstance == null)
        {
            batteryNormalInstance = Instantiate(batteryNormalPrefab);
        }

        if (batteryPosition.x >= 0 && batteryPosition.y >= 0)
        {
            Vector3 batteryWorldPosition = gridGenerator.GetGridPoints()[batteryPosition.x, batteryPosition.y];
            float scaleFactor = gridGenerator.gridSpacing * 0.25f;

            batteryNormalInstance.transform.position = batteryWorldPosition;
            batteryNormalInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            batteryNormalInstance.SetActive(true);
        }
        else
        {
            batteryNormalInstance.SetActive(false);
        }
    }

    bool IsAmmeterOnBoundary(Vector2Int pos)
    {
        if (pos.x == 0 && pos.y >= 0 && pos.y <= 5) return true;
        if (pos.x == 2 && pos.y >= 0 && pos.y <= 5) return true;
        if (pos.y == 0 && pos.x >= 0 && pos.x <= 2) return true;
        if (pos.y == 6 && pos.x >= 0 && pos.x <= 2) return true;
        return false;
    }

    public void UpdateAmmeterModel(Vector2Int ammeterPos)
    {
        if (ammeterPos.x < 0 || ammeterPos.y < 0)
        {
            if (ammeterInstance != null)
            {
                Destroy(ammeterInstance);
                ammeterInstance = null;
            }
            return;
        }

        bool ammeterHasReading = IsAmmeterOnBoundary(ammeterPos);

        if (ammeterInstance != null)
        {
            Destroy(ammeterInstance);
            ammeterInstance = null;
        }

        Vector3 ammeterWorldPos = gridGenerator.GetGridPoints()[ammeterPos.x, ammeterPos.y];
        float scaleFactor = gridGenerator.gridSpacing * 0.25f;

        if (ammeterHasReading)
        {
            ammeterInstance = Instantiate(ammeterReadingPrefab, ammeterWorldPos, Quaternion.identity);
        }
        else
        {
            ammeterInstance = Instantiate(ammeterNormalPrefab, ammeterWorldPos, Quaternion.identity);
        }
        ammeterInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

    public void UpdateVoltmeterModel(Vector2Int voltmeterPos)
    {
        if (voltmeterPos.x < 0 || voltmeterPos.y < 0)
        {
            if (voltmeterInstance != null)
            {
                Destroy(voltmeterInstance);
                voltmeterInstance = null;
            }
            return;
        }

        bool hasReading = (voltmeterPos.x == 2 && voltmeterPos.y == 1);

        if (voltmeterInstance != null)
        {
            Destroy(voltmeterInstance);
            voltmeterInstance = null;
        }

        Vector3 voltmeterWorldPos = gridGenerator.GetGridPoints()[voltmeterPos.x, voltmeterPos.y];
        float scaleFactor = gridGenerator.gridSpacing * 0.25f;

        if (hasReading)
        {
            voltmeterInstance = Instantiate(voltmeterReadingPrefab, voltmeterWorldPos, Quaternion.identity);
        }
        else
        {
            voltmeterInstance = Instantiate(voltmeterNormalPrefab, voltmeterWorldPos, Quaternion.identity);
        }
        voltmeterInstance.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }


    public void SetPuzzleModeStarted(bool started)
    {
        puzzleModeStarted = started;
    }

    private void SetWireMaterial(Material material)
    {
        foreach (var wire in wires)
        {
            wire.GetComponent<Renderer>().material = material;
        }
    }
}
