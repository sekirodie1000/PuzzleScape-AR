using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int rows = 4;
    public int cols = 6;
    public float cellSize = 1f;
    public Vector3 originPosition = Vector3.zero;
    public Material poweredMaterial; // 电流通过时的材质
    public Material defaultMaterial; // 默认未通电的材质

    // 表示棋盘格的二维数组
    public GridCell[,] grid;

    void Awake()
    {
        Instance = this;
        grid = new GridCell[rows, cols];

        // 初始化网格单元
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

    // 开始电流流动
    public void StartCurrentFlow()
    {
        // 找到电池的位置
        Vector2Int batteryPos = FindComponent(ComponentType.Battery);
        if (batteryPos == Vector2Int.one * -1)
        {
            Debug.Log("couldn't find battery");
            return;
        }

        // 重置电流流动
        ResetCurrentFlow();

        // 电流向左和向右延伸
        FlowCurrent(batteryPos, Direction.Left);
        FlowCurrent(batteryPos, Direction.Right);

        // 检查灯泡是否通电
        CheckBulbPowered();
    }

    // 查找特定类型的组件
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

    // 重置电流流动
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
                        renderer.material = defaultMaterial; // 设置为默认材质
                    }
                    grid[x, y].component.ResetState();
                }
            }
        }
    }

    // 递归函数，实现电流流动
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
                renderer.material = poweredMaterial; // 设置为电流材质
            }

            // 处理开关
            if (cell.component.componentType == ComponentType.Switch)
            {
                // 如果开关未闭合，停止电流
                if (!cell.component.isClosed)
                {
                    return;
                }
            }
            else if (cell.component.componentType == ComponentType.Bulb)
            {
                // 灯泡被供电，可以停止递归，或者继续延伸电流
                return;
            }
        }

        // 计算下一个位置
        Vector2Int nextPos = GetNextPosition(pos, dir);

        // 在拐角处自动转弯
        if (IsCornerPosition(pos))
        {
            // 转弯90度
            Direction newDir = TurnDirection(dir);
            nextPos = GetNextPosition(pos, newDir);
            dir = newDir;
        }

        // 继续电流流动
        FlowCurrent(nextPos, dir);
    }

    // 判断当前位置是否是拐角
    bool IsCornerPosition(Vector2Int pos)
    {
        // 根据您的棋盘格设计，指定哪些位置是拐角
        // 示例：如果棋盘格的四个角是拐角
        return (pos.x == 0 && pos.y == 0) ||
               (pos.x == 0 && pos.y == cols - 1) ||
               (pos.x == rows - 1 && pos.y == 0) ||
               (pos.x == rows - 1 && pos.y == cols - 1);
    }

    // 转弯90度
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

    // 辅助函数
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

    // 检查灯泡是否被供电
    void CheckBulbPowered()
    {
        // 找到灯泡的位置
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

// 其他类
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

