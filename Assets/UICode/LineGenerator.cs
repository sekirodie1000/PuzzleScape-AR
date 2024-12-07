using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;        // ���ߵ�Ԥ����
    public Transform startPoint;        // ���ݵ����
    public Transform cornerPoint;       // �սǵ�
    public float extensionLength = 3f;  // �ӹս�����Ĺ̶�����

    private GameObject firstLine;       // ��һ�ε���
    private GameObject secondLine;      // �ڶ��ε���

    void Start()
    {
        if (linePrefab == null || startPoint == null || cornerPoint == null)
        {
            Debug.LogError("LinePrefab, StartPoint, or CornerPoint is not assigned!");
            return;
        }

        // ���ɵ�һ�ε��ߣ��ӵ��ݵ��ս�
        CreateLine(startPoint.position, cornerPoint.position, ref firstLine);

        // ��̬������һ���λ��
        Vector3 nextPointPosition = CalculateNextPoint(cornerPoint.position, cornerPoint.forward, extensionLength);

        // ���ɵڶ��ε��ߣ��ӹսǵ������
        CreateLine(cornerPoint.position, nextPointPosition, ref secondLine);
    }

    void CreateLine(Vector3 pointA, Vector3 pointB, ref GameObject line)
    {
        // �����м�λ�úͷ���
        Vector3 midPoint = (pointA + pointB) / 2;
        Vector3 direction = pointB - pointA;
        float length = direction.magnitude;

        // ��������
        line = Instantiate(linePrefab, midPoint, Quaternion.identity);
        line.transform.right = direction.normalized;   // ���õ��߷���
        line.transform.localScale = new Vector3(length, 0.1f, 0.1f); // ���õ��߳���
        Debug.Log($"Line created between {pointA} and {pointB}, length: {length}");
    }

    Vector3 CalculateNextPoint(Vector3 origin, Vector3 direction, float length)
    {
        // ���ݹսǵ��λ�úͷ��򣬼���������λ��
        return origin + direction.normalized * length;
    }

    public void DestroyLines()
    {
        // ɾ�����ɵĵ���
        if (firstLine != null) Destroy(firstLine);
        if (secondLine != null) Destroy(secondLine);
        Debug.Log("All lines destroyed.");
    }
}
