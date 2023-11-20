using Godot;


public partial class GridSystem : Node
{
    [Export]
    private Vector3 cellSize = new Vector3(1, 1, 1);

    static Godot.Collections.Dictionary<Vector3I, Cell> grid = new Godot.Collections.Dictionary<Vector3I, Cell>();

    public static void setPosition(Vector3 newPos, Cell cell)
    {
        //Sets the position of the cell to the centered pos
        cell.node.Position = translateToRelativePos(newPos);
        
        //Converts to grid pos for the dictionary
        Vector3I gridPos = translateToGridPos(newPos);
        if (grid.ContainsKey(gridPos))
        {
            grid[gridPos] = cell;
        }
        else
        {
            grid.Add(gridPos, cell);
        }
    }

    public static Godot.Collections.Dictionary<Vector3I, Cell> getGrid()
    {
        return grid;
    }

    public static Cell getCell(Vector3I pos)
    {
        if (grid.ContainsKey(pos)) return grid[pos];
        else return null;
    }

    public static Vector3I getGridPosition(Cell cell)
    {
        return (Vector3I) cell.node.Position;
    }

    public static Vector3 translateToRelativePos(Vector3 pos)
    {
        return pos.Floor() + new Vector3(1, 1, 1) * 0.5f;
    }
    
    public static Vector3I translateToGridPos(Vector3 pos)
    {
        return (Vector3I) pos;
    }
}
