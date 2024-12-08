using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleCheckUI : MonoBehaviour
{
    public WireGenerator1 wireGenerator;
    public ARMarkerDetector1 markerDetector;
    public Button checkButton;

    void Start()
    {
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(OnCheckButtonClicked);
        }
    }

    public void OnCheckButtonClicked()
    {
        List<Vector2Int> lampPositions = markerDetector.GetLampPositions();
        Vector2Int batteryPos = markerDetector.GetBatteryPosition();
        Vector2Int switchPos = markerDetector.GetSwitchPosition();

        wireGenerator.CheckCircuitCompletion(lampPositions, batteryPos, switchPos);
    }
}
