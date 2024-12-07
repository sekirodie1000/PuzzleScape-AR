using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class ARMarkerDetector : MonoBehaviour
{
    public VirtualGridGenerator gridGenerator;
    public WireGenerator wireGenerator;

    public ObserverBehaviour[] lampObservers; // Array of lamp observers
    public ObserverBehaviour batteryObserver;
    public ObserverBehaviour switchObserver; // Add this line

    private List<Vector2Int> lampPositions = new List<Vector2Int>();
    private Vector2Int batteryPosition = new Vector2Int(-1, -1);
    private Vector2Int switchPosition = new Vector2Int(-1, -1); // Add this line

    void Update()
    {
        lampPositions.Clear(); // Clear previous positions

        // Update lamp positions
        foreach (var lampObserver in lampObservers)
        {
            if (lampObserver.TargetStatus.Status == Status.TRACKED)
            {
                Vector2Int currentLampPosition = gridGenerator.GetNearestGridPoint(lampObserver.transform.position);
                lampPositions.Add(currentLampPosition);
                wireGenerator.UpdateLampModel(currentLampPosition, lampObserver.GetInstanceID());
            }
            else
            {
                // Remove lamp model when not tracked
                wireGenerator.UpdateLampModel(new Vector2Int(-1, -1), lampObserver.GetInstanceID());
            }
        }

        // Update battery position
        if (batteryObserver.TargetStatus.Status == Status.TRACKED)
        {
            batteryPosition = gridGenerator.GetNearestGridPoint(batteryObserver.transform.position);
            wireGenerator.UpdateBatteryModel(batteryPosition, switchPosition);
        }
        else
        {
            batteryPosition = new Vector2Int(-1, -1);
            wireGenerator.UpdateBatteryModel(batteryPosition, switchPosition);
        }

        // Update switch position
        if (switchObserver.TargetStatus.Status == Status.TRACKED)
        {
            switchPosition = gridGenerator.GetNearestGridPoint(switchObserver.transform.position);
            wireGenerator.UpdateSwitchModel(switchPosition);
        }
        else
        {
            switchPosition = new Vector2Int(-1, -1);
            wireGenerator.UpdateSwitchModel(switchPosition);
        }

        // Update circuit and check completion
        wireGenerator.UpdateCircuit(lampPositions, batteryPosition, switchPosition, gridGenerator.rows, gridGenerator.cols);
        wireGenerator.CheckCircuitCompletion(lampPositions, batteryPosition, switchPosition);
    }
}
