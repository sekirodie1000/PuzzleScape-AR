using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class ARMarkerDetector : MonoBehaviour
{
    public VirtualGridGenerator gridGenerator;
    public WireGenerator wireGenerator;

    public ObserverBehaviour[] lampObservers;
    public ObserverBehaviour batteryObserver;
    public ObserverBehaviour switchObserver;

    private List<Vector2Int> lampPositions = new List<Vector2Int>();
    private Vector2Int batteryPosition = new Vector2Int(-1, -1);
    private Vector2Int switchPosition = new Vector2Int(-1, -1);

    void Update()
    {
        lampPositions.Clear();

        foreach (var lampObserver in lampObservers)
        {
            if (lampObserver.TargetStatus.Status == Status.TRACKED || lampObserver.TargetStatus.Status == Status.EXTENDED_TRACKED)
            {
                Vector2Int currentLampPosition = gridGenerator.GetNearestGridPoint(lampObserver.transform.position);
                lampPositions.Add(currentLampPosition);
                wireGenerator.UpdateLampModel(currentLampPosition, lampObserver.GetInstanceID());
            }
            else
            {
                wireGenerator.UpdateLampModel(new Vector2Int(-1, -1), lampObserver.GetInstanceID());
            }
        }

        if (batteryObserver.TargetStatus.Status == Status.TRACKED || batteryObserver.TargetStatus.Status == Status.EXTENDED_TRACKED)
        {
            batteryPosition = gridGenerator.GetNearestGridPoint(batteryObserver.transform.position);
            wireGenerator.UpdateBatteryModel(batteryPosition, switchPosition);
        }
        else
        {
            batteryPosition = new Vector2Int(-1, -1);
            wireGenerator.UpdateBatteryModel(batteryPosition, switchPosition);
        }

        if (switchObserver.TargetStatus.Status == Status.TRACKED || switchObserver.TargetStatus.Status == Status.EXTENDED_TRACKED)
        {
            switchPosition = gridGenerator.GetNearestGridPoint(switchObserver.transform.position);
            wireGenerator.UpdateSwitchModel(switchPosition);
        }
        else
        {
            switchPosition = new Vector2Int(-1, -1);
            wireGenerator.UpdateSwitchModel(switchPosition);
        }

        wireGenerator.UpdateCircuit(lampPositions, batteryPosition, switchPosition, gridGenerator.rows, gridGenerator.cols);
        wireGenerator.CheckCircuitCompletion(lampPositions, batteryPosition, switchPosition);
    }
}
