using UnityEngine;
using Vuforia;

public class PlaceComponent : MonoBehaviour
{
    public GameObject modelPrefab;
    private GameObject instantiatedModel;

    void Start()
    {
        var observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (instantiatedModel == null)
            {
                // �����̸���ʵ����ģ��
                Vector2Int gridPos = GridManager.Instance.GetGridPosition(transform.position);
                Vector3 worldPos = GridManager.Instance.GetWorldPosition(gridPos.x, gridPos.y);
                instantiatedModel = Instantiate(modelPrefab, worldPos, Quaternion.identity);
                instantiatedModel.transform.parent = this.transform;

                // ��ӵ�����
                GridCell cell = GridManager.Instance.grid[gridPos.x, gridPos.y];
                cell.component = instantiatedModel.GetComponent<CircuitComponent>();

                // ��ʼ��������
                GridManager.Instance.StartCurrentFlow();
            }
        }
        else
        {
            if (instantiatedModel != null)
            {
                // ���������Ƴ�
                Vector2Int gridPos = GridManager.Instance.GetGridPosition(instantiatedModel.transform.position);
                GridManager.Instance.grid[gridPos.x, gridPos.y].component = null;
                Destroy(instantiatedModel);
                instantiatedModel = null;

                // ���õ�������
                GridManager.Instance.StartCurrentFlow();
            }
        }
    }
}
