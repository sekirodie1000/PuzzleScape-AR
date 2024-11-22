using UnityEngine;

public class CornerDetection : MonoBehaviour
{
    public Transform startPoint;       // ���ݵ����
    public Transform cornerPoint;      // �սǵ�
    public LineManager lineManager;    // ���߹�����
    public float detectionRadius = 0.5f; // ���뾶

    private bool cornerConnected = false; // ȷ�������߼�ִֻ��һ��

    void Update()
    {
        if (cornerConnected || startPoint == null || cornerPoint == null || lineManager == null) return;

        // �����ݵ�����Ƿ񿿽��սǵ�
        float distanceToCorner = Vector3.Distance(startPoint.position, cornerPoint.position);
        if (distanceToCorner <= detectionRadius)
        {
            Debug.Log("Corner detected! Connecting line...");
            lineManager.CreateLineBetween(startPoint, cornerPoint); // ������������
            cornerConnected = true; // ��ֹ�ظ�����
        }
    }
}
