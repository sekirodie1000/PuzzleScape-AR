using UnityEngine;
using Vuforia;

public class SetGridOrigin : MonoBehaviour
{
    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            // ���������ԭ��λ��
            GridManager.Instance.SetOriginPosition(transform.position);
        }
    }
}
