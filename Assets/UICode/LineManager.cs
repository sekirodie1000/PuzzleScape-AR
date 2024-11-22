using UnityEngine;

public class LineManager : MonoBehaviour
{
    public GameObject linePrefab;  // ��ɫ���ߵ�Ԥ����
    private GameObject currentLine; // ��̬���ɵĵ���

    // ��̬���ɵ��ߣ����� startPoint �� cornerPoint
    public void CreateLineBetween(Transform startPoint, Transform cornerPoint)
    {
        if (linePrefab == null || startPoint == null || cornerPoint == null)
        {
            Debug.LogError("Line Prefab, Start Point, or Corner Point is not assigned!");
            return;
        }

        // ���������յ���м�λ��
        Vector3 midPoint = (startPoint.position + cornerPoint.position) / 2;

        // ���㷽��ͳ���
        Vector3 direction = cornerPoint.position - startPoint.position;
        float length = direction.magnitude;

        // ʵ��������
        currentLine = Instantiate(linePrefab, midPoint, Quaternion.identity);
        currentLine.transform.right = direction.normalized; // �������߷���
        currentLine.transform.localScale = new Vector3(length, 0.1f, 0.1f); // ���õ��߳���

        Debug.Log($"Line created between {startPoint.position} and {cornerPoint.position}, length: {length}");
    }

    // ɾ����ǰ����
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
