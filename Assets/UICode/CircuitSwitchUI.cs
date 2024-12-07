using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CircuitSwitchUI : MonoBehaviour
{
    public WireGenerator wireGenerator;
    public Button switchCircuitButton;
    public TMP_Text buttonText;

    void Start()
    {
        if (switchCircuitButton != null)
        {
            switchCircuitButton.onClick.AddListener(ToggleCircuitType);
        }

        UpdateButtonText();
    }

    public void ToggleCircuitType()
    {
        if (wireGenerator.currentCircuitType == WireGenerator.CircuitType.Series)
        {
            wireGenerator.currentCircuitType = WireGenerator.CircuitType.Parallel;
        }
        else
        {
            wireGenerator.currentCircuitType = WireGenerator.CircuitType.Series;
        }

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = wireGenerator.currentCircuitType == WireGenerator.CircuitType.Series
                ? "Parallel"
                : "Series";
        }
    }
}
