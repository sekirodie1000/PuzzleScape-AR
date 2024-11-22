using UnityEngine;

public class CornerDetection : MonoBehaviour
{
    public Transform startPoint;       // 灯泡的起点
    public Transform cornerPoint;      // 拐角点
    public LineManager lineManager;    // 电线管理器
    public float detectionRadius = 0.5f; // 检测半径

    private bool cornerConnected = false; // 确保连接逻辑只执行一次

    void Update()
    {
        if (cornerConnected || startPoint == null || cornerPoint == null || lineManager == null) return;

        // 检测灯泡的起点是否靠近拐角点
        float distanceToCorner = Vector3.Distance(startPoint.position, cornerPoint.position);
        if (distanceToCorner <= detectionRadius)
        {
            Debug.Log("Corner detected! Connecting line...");
            lineManager.CreateLineBetween(startPoint, cornerPoint); // 创建电线连接
            cornerConnected = true; // 防止重复连接
        }
    }
}
