using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    public float floatSpeed = 1f; // Speed of the floating
    public float floatHeight = 10f; // Height of the floating

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; // Save the starting position
    }

    void Update()
    {
        // Update the Y position to make it float
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
