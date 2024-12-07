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
                // 在棋盘格上实例化模型
                Vector2Int gridPos = GridManager.Instance.GetGridPosition(transform.position);
                Vector3 worldPos = GridManager.Instance.GetWorldPosition(gridPos.x, gridPos.y);
                instantiatedModel = Instantiate(modelPrefab, worldPos, Quaternion.identity);
                instantiatedModel.transform.parent = this.transform;

                // 添加到网格
                GridCell cell = GridManager.Instance.grid[gridPos.x, gridPos.y];
                cell.component = instantiatedModel.GetComponent<CircuitComponent>();

                // 开始电流流动
                GridManager.Instance.StartCurrentFlow();
            }
        }
        else
        {
            if (instantiatedModel != null)
            {
                // 从网格中移除
                Vector2Int gridPos = GridManager.Instance.GetGridPosition(instantiatedModel.transform.position);
                GridManager.Instance.grid[gridPos.x, gridPos.y].component = null;
                Destroy(instantiatedModel);
                instantiatedModel = null;

                // 重置电流流动
                GridManager.Instance.StartCurrentFlow();
            }
        }
    }
}
