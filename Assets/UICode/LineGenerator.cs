using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;        // 电线的预制体
    public Transform startPoint;        // 灯泡的起点
    public Transform cornerPoint;       // 拐角点
    public float extensionLength = 3f;  // 从拐角延伸的固定长度

    private GameObject firstLine;       // 第一段电线
    private GameObject secondLine;      // 第二段电线

    void Start()
    {
        if (linePrefab == null || startPoint == null || cornerPoint == null)
        {
            Debug.LogError("LinePrefab, StartPoint, or CornerPoint is not assigned!");
            return;
        }

        // 生成第一段电线，从灯泡到拐角
        CreateLine(startPoint.position, cornerPoint.position, ref firstLine);

        // 动态计算下一点的位置
        Vector3 nextPointPosition = CalculateNextPoint(cornerPoint.position, cornerPoint.forward, extensionLength);

        // 生成第二段电线，从拐角到延伸点
        CreateLine(cornerPoint.position, nextPointPosition, ref secondLine);
    }

    void CreateLine(Vector3 pointA, Vector3 pointB, ref GameObject line)
    {
        // 计算中间位置和方向
        Vector3 midPoint = (pointA + pointB) / 2;
        Vector3 direction = pointB - pointA;
        float length = direction.magnitude;

        // 创建电线
        line = Instantiate(linePrefab, midPoint, Quaternion.identity);
        line.transform.right = direction.normalized;   // 设置电线方向
        line.transform.localScale = new Vector3(length, 0.1f, 0.1f); // 设置电线长度
        Debug.Log($"Line created between {pointA} and {pointB}, length: {length}");
    }

    Vector3 CalculateNextPoint(Vector3 origin, Vector3 direction, float length)
    {
        // 根据拐角点的位置和方向，计算延伸点的位置
        return origin + direction.normalized * length;
    }

    public void DestroyLines()
    {
        // 删除生成的电线
        if (firstLine != null) Destroy(firstLine);
        if (secondLine != null) Destroy(secondLine);
        Debug.Log("All lines destroyed.");
    }
}
