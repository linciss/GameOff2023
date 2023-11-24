using GameOff2023.entities;
using GameOff2023.entities.placeable;
using Godot;


public partial class GridSystem : Node
{
    [Export]
    private Vector3 cellSize = new Vector3(1, 1, 1);

    static Godot.Collections.Dictionary<Vector3I, Cell> grid = new Godot.Collections.Dictionary<Vector3I, Cell>();

    public static void setPosition(Vector3 newPos, ICellItem cellItem)
    {
        //Gets cell at the new position if doesnt exist makes it automatically
        Cell targetCell = getCell(translateToGridPos(newPos));
        //Sets the position of the cell's node to the centered pos
        cellItem.getNode().Position = translateToRelativePos(newPos);
        
        targetCell.setCellItem(cellItem);
    }

    public static Godot.Collections.Dictionary<Vector3I, Cell> getGrid()
    {
        return grid;
    }

    public static Cell getCell(Vector3I pos)
    {
        Cell cell;
        if (grid.TryGetValue(pos, out cell))
        {
            return cell;
        }
        cell = new Cell(pos);
        grid.Add(pos, cell);
        return cell;
    }

    public static Vector3 translateToRelativePos(Vector3 pos)
    {
        return (pos.Floor() + Vector3.One * 0.5f);  
    }
    
    public static Vector3I translateToGridPos(Vector3 pos)
    {
        return (Vector3I) pos;
    }
}
