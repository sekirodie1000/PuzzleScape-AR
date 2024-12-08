using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class ARMarkerDetector1 : MonoBehaviour
{
    public VirtualGridGenerator gridGenerator;
    public WireGenerator1 wireGenerator;

    public ObserverBehaviour[] lampObservers;
    public ObserverBehaviour batteryObserver;
    public ObserverBehaviour switchObserver;
    public ObserverBehaviour ammeterObserver;

    public bool isLevel2 = false;
    public ObserverBehaviour voltmeterObserver;
    public ObserverBehaviour brokenLampObserver;
    public ObserverBehaviour goodLampObserver;

    private List<Vector2Int> lampPositions = new List<Vector2Int>();
    private Vector2Int batteryPosition = new Vector2Int(-1, -1);
    private Vector2Int switchPosition = new Vector2Int(-1, -1);
    private Vector2Int ammeterPosition = new Vector2Int(-1, -1);
    private Vector2Int voltmeterPosition = new Vector2Int(-1, -1);

    private Dictionary<int, bool> lampTypeMap = new Dictionary<int, bool>();
    private Dictionary<Vector2Int, int> lampPositionToID = new Dictionary<Vector2Int, int>();

    void Start()
    {
        foreach (var lo in lampObservers)
        {
            lampTypeMap[lo.GetInstanceID()] = true;
        }

        if (brokenLampObserver != null)
            lampTypeMap[brokenLampObserver.GetInstanceID()] = false;

        if (goodLampObserver != null)
            lampTypeMap[goodLampObserver.GetInstanceID()] = true;
    }

    void Update()
    {
        lampPositions.Clear();
        lampPositionToID.Clear();

        foreach (var lampObserver in lampObservers)
        {
            if (lampObserver.TargetStatus.Status == Status.TRACKED)
            {
                Vector2Int lp = gridGenerator.GetNearestGridPoint(lampObserver.transform.position);
                lampPositions.Add(lp);
                wireGenerator.UpdateLampModel(lp, lampObserver.GetInstanceID());
                lampPositionToID[lp] = lampObserver.GetInstanceID();
            }
            else
            {
                wireGenerator.UpdateLampModel(new Vector2Int(-1, -1), lampObserver.GetInstanceID());
            }
        }

        if (isLevel2)
        {
            if (brokenLampObserver != null)
            {
                if (brokenLampObserver.TargetStatus.Status == Status.TRACKED)
                {
                    Vector2Int lp = gridGenerator.GetNearestGridPoint(brokenLampObserver.transform.position);
                    lampPositions.Add(lp);
                    wireGenerator.UpdateLampModel(lp, brokenLampObserver.GetInstanceID());
                    lampPositionToID[lp] = brokenLampObserver.GetInstanceID();
                }
                else
                {
                    wireGenerator.UpdateLampModel(new Vector2Int(-1, -1), brokenLampObserver.GetInstanceID());
                }
            }

            if (goodLampObserver != null)
            {
                if (goodLampObserver.TargetStatus.Status == Status.TRACKED)
                {
                    Vector2Int lp = gridGenerator.GetNearestGridPoint(goodLampObserver.transform.position);
                    lampPositions.Add(lp);
                    wireGenerator.UpdateLampModel(lp, goodLampObserver.GetInstanceID());
                    lampPositionToID[lp] = goodLampObserver.GetInstanceID();
                }
                else
                {
                    wireGenerator.UpdateLampModel(new Vector2Int(-1, -1), goodLampObserver.GetInstanceID());
                }
            }
        }

        if (batteryObserver.TargetStatus.Status == Status.TRACKED)
        {
            batteryPosition = gridGenerator.GetNearestGridPoint(batteryObserver.transform.position);
        }
        else
        {
            batteryPosition = new Vector2Int(-1, -1);
        }
        wireGenerator.UpdateBatteryModel(batteryPosition, switchPosition);

        if (switchObserver.TargetStatus.Status == Status.TRACKED)
        {
            switchPosition = gridGenerator.GetNearestGridPoint(switchObserver.transform.position);
        }
        else
        {
            switchPosition = new Vector2Int(-1, -1);
        }
        wireGenerator.UpdateSwitchModel(switchPosition);

        if (ammeterObserver != null)
        {
            if (ammeterObserver.TargetStatus.Status == Status.TRACKED)
            {
                ammeterPosition = gridGenerator.GetNearestGridPoint(ammeterObserver.transform.position);
            }
            else
            {
                ammeterPosition = new Vector2Int(-1, -1);
            }
            wireGenerator.UpdateAmmeterModel(ammeterPosition);
        }

        if (isLevel2 && voltmeterObserver != null)
        {
            if (voltmeterObserver.TargetStatus.Status == Status.TRACKED)
            {
                voltmeterPosition = gridGenerator.GetNearestGridPoint(voltmeterObserver.transform.position);
            }
            else
            {
                voltmeterPosition = new Vector2Int(-1, -1);
            }
            wireGenerator.UpdateVoltmeterModel(voltmeterPosition);
        }

        wireGenerator.UpdateCircuit(lampPositions, batteryPosition, switchPosition, gridGenerator.rows, gridGenerator.cols);
    }

    public List<Vector2Int> GetLampPositions() { return new List<Vector2Int>(lampPositions); }
    public Vector2Int GetBatteryPosition() { return batteryPosition; }
    public Vector2Int GetSwitchPosition() { return switchPosition; }
    public Vector2Int GetAmmeterPosition() { return ammeterPosition; }

    public int GetLampInstanceIDAtPosition(Vector2Int pos)
    {
        if (lampPositionToID.ContainsKey(pos))
            return lampPositionToID[pos];
        return -1;
    }

    public bool IsLampGood(int instanceID)
    {
        if (instanceID == -1) return false;
        if (lampTypeMap.ContainsKey(instanceID))
            return lampTypeMap[instanceID];
        return false;
    }
}
