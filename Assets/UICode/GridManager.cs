using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int rows = 4;
    public int cols = 6;
    public float cellSize = 1f;
    public Vector3 originPosition = Vector3.zero;
    public Material poweredMaterial; // ����ͨ��ʱ�Ĳ���
    public Material defaultMaterial; // Ĭ��δͨ��Ĳ���

    // ��ʾ���̸�Ķ�ά����
    public GridCell[,] grid;

    void Awake()
    {
        Instance = this;
        grid = new GridCell[rows, cols];

        // ��ʼ������Ԫ
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                grid[x, y] = new GridCell();
            }
        }
    }

    public void SetOriginPosition(Vector3 position)
    {
        originPosition = position;
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - originPosition.x) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.z - originPosition.z) / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * cellSize + originPosition.x, originPosition.y, y * cellSize + originPosition.z);
    }

    // ��ʼ��������
    public void StartCurrentFlow()
    {
        // �ҵ���ص�λ��
        Vector2Int batteryPos = FindComponent(ComponentType.Battery);
        if (batteryPos == Vector2Int.one * -1)
        {
            Debug.Log("couldn't find battery");
            return;
        }

        // ���õ�������
        ResetCurrentFlow();

        // �����������������
        FlowCurrent(batteryPos, Direction.Left);
        FlowCurrent(batteryPos, Direction.Right);

        // �������Ƿ�ͨ��
        CheckBulbPowered();
    }

    // �����ض����͵����
    Vector2Int FindComponent(ComponentType type)
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                if (grid[x, y].component != null && grid[x, y].component.componentType == type)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return Vector2Int.one * -1;
    }

    // ���õ�������
    void ResetCurrentFlow()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                grid[x, y].isPowered = false;
                if (grid[x, y].component != null)
                {
                    Renderer renderer = grid[x, y].visualObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = defaultMaterial; // ����ΪĬ�ϲ���
                    }
                    grid[x, y].component.ResetState();
                }
            }
        }
    }

    // �ݹ麯����ʵ�ֵ�������
    void FlowCurrent(Vector2Int pos, Direction dir)
    {
        if (!IsValidPosition(pos))
            return;

        GridCell cell = grid[pos.x, pos.y];
        cell.isPowered = true;

        if (cell.component != null)
        {
            cell.component.OnPowered();
            Renderer renderer = cell.visualObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material = poweredMaterial; // ����Ϊ��������
            }

            // ������
            if (cell.component.componentType == ComponentType.Switch)
            {
                // �������δ�պϣ�ֹͣ����
                if (!cell.component.isClosed)
                {
                    return;
                }
            }
            else if (cell.component.componentType == ComponentType.Bulb)
            {
                // ���ݱ����磬����ֹͣ�ݹ飬���߼����������
                return;
            }
        }

        // ������һ��λ��
        Vector2Int nextPos = GetNextPosition(pos, dir);

        // �ڹսǴ��Զ�ת��
        if (IsCornerPosition(pos))
        {
            // ת��90��
            Direction newDir = TurnDirection(dir);
            nextPos = GetNextPosition(pos, newDir);
            dir = newDir;
        }

        // ������������
        FlowCurrent(nextPos, dir);
    }

    // �жϵ�ǰλ���Ƿ��ǹս�
    bool IsCornerPosition(Vector2Int pos)
    {
        // �����������̸���ƣ�ָ����Щλ���ǹս�
        // ʾ����������̸���ĸ����ǹս�
        return (pos.x == 0 && pos.y == 0) ||
               (pos.x == 0 && pos.y == cols - 1) ||
               (pos.x == rows - 1 && pos.y == 0) ||
               (pos.x == rows - 1 && pos.y == cols - 1);
    }

    // ת��90��
    Direction TurnDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                return Direction.Up;
            case Direction.Right:
                return Direction.Down;
            case Direction.Up:
                return Direction.Right;
            case Direction.Down:
                return Direction.Left;
            default:
                return dir;
        }
    }

    // ��������
    Vector2Int GetNextPosition(Vector2Int pos, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return new Vector2Int(pos.x, pos.y + 1);
            case Direction.Down: return new Vector2Int(pos.x, pos.y - 1);
            case Direction.Left: return new Vector2Int(pos.x - 1, pos.y);
            case Direction.Right: return new Vector2Int(pos.x + 1, pos.y);
            default: return pos;
        }
    }

    bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < rows && pos.y >= 0 && pos.y < cols;
    }

    // �������Ƿ񱻹���
    void CheckBulbPowered()
    {
        // �ҵ����ݵ�λ��
        Vector2Int bulbPos = FindComponent(ComponentType.Bulb);
        if (bulbPos == Vector2Int.one * -1)
        {
            Debug.Log("couldn't finf lamp");
            return;
        }

        if (grid[bulbPos.x, bulbPos.y].isPowered)
        {
            Debug.Log("lamp work!");
            grid[bulbPos.x, bulbPos.y].component.OnPowered();
        }
        else
        {
            Debug.Log("lamp NOT work");
        }
    }
}

// ������
public class GridCell
{
    public CircuitComponent component;
    public bool isPowered;
    public GameObject visualObject;
}

public enum Direction
{
    Up, Down, Left, Right
}

