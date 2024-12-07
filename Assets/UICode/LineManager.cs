using UnityEngine;

public class LineManager : MonoBehaviour
{
    public GameObject linePrefab;  // 灰色电线的预制体
    private GameObject currentLine; // 动态生成的电线

    public void CreateLineBetween(Transform startPoint, Transform cornerPoint)
    {
        if (linePrefab == null || startPoint == null || cornerPoint == null)
        {
            Debug.LogError("Line Prefab, Start Point, or Corner Point is not assigned!");
            return;
        }

        // 计算起点和终点的中间位置
        Vector3 midPoint = (startPoint.position + cornerPoint.position) / 2;

        // 计算方向和长度
        Vector3 direction = cornerPoint.position - startPoint.position;
        float length = direction.magnitude;

        // 实例化电线
        currentLine = Instantiate(linePrefab, midPoint, Quaternion.identity);

        // 调整电线的方向和位置
        currentLine.transform.position = midPoint;
        currentLine.transform.rotation = Quaternion.LookRotation(direction.normalized);

        // 设置电线的缩放
        currentLine.transform.localScale = new Vector3(0.1f, 0.1f, length); // 如果电线沿 Z 轴延伸

        Debug.Log($"Line created between {startPoint.position} and {cornerPoint.position}, length: {length}");
    }

    public void DestroyLine()
    {
        if (currentLine != null)
        {
            Destroy(currentLine);
            currentLine = null;
            Debug.Log("Line destroyed.");
        }
    }
}
