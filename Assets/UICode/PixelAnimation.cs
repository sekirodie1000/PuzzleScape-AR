using UnityEngine;

public class PixelAnimation : MonoBehaviour
{
    public float shakeSpeed = 5f;  // Speed of the shake (rotations per second)
    public float maxAngle = 10f;  // Maximum rotation angle in degrees
    public float pixelStep = 1f;  // Angle snapping for pixelated effect

    private float currentAngle = 0f; // The current rotation angle

    void Update()
    {
        // Calculate the raw angle based on a sine wave
        float rawAngle = Mathf.Sin(Time.time * shakeSpeed * Mathf.PI * 2) * maxAngle;

        // Snap the angle to the nearest step size for a pixelated effect
        currentAngle = Mathf.Round(rawAngle / pixelStep) * pixelStep;

        // Apply the rotation to the Z axis
        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
