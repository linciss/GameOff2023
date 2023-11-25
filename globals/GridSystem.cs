using System;
using GameOff2023.entities;
using GameOff2023.entities.placeable;
using GameOff2023.random;
using Godot;
using Godot.Collections;


public partial class GridSystem : Node3D
{
    [Export]
    private Vector3 cellSize = new Vector3(1, 1, 1);

    static Dictionary<Vector3I, Cell> grid = new Godot.Collections.Dictionary<Vector3I, Cell>();

    private float passedTime = 0f;
    
    GridSystem()
    {
        Vector3 test = new Vector3(1.5f, 1.5f, 1.5f);
        GD.Print("test: " + test);
        test=translateToRelativePos(test);
        GD.Print("Best: " + test);
        
        Vector3I test2 = new Vector3I(-1, 1, 1);
        GD.Print("test2: " + test2);
        test2=translateToGridPos(test2);
        GD.Print("Best2: " + test2);
        
    }
    public override void _Process(double delta)
    {
        if (passedTime > getTickSpeed())
        {
            passedTime = 0f;
           // tickPlaceables();
        }
        passedTime += (float) delta;
    }

    public static void setPosition(Vector3 newPos, ICellItem cellItem)
    {
        //Gets cell at the new position if doesnt exist makes it automatically
        Cell targetCell = getCell(translateToGridPos(newPos));
        //Sets the position of the cell's node to the centered pos
        cellItem.getNode().Position = translateToRelativePos(targetCell.getPosition());
        
        targetCell.setCellItem(cellItem);
    }

    public static Godot.Collections.Dictionary<Vector3I, Cell> getGrid()
    {
        return grid;
    }

    public static Cell getCell(Vector3I pos)
    {
        if (grid.TryGetValue(pos, out Cell cell))
        {
            return cell;
        }
        cell = new Cell(pos);
        grid.Add(pos, cell);
        return cell;
    }
    
    public static Vector3 translateToRelativePos(Vector3 pos)
    {
        pos = pos.Floor();
        return pos.Floor() + Vector3.One * 0.5f;  
        // Math.Sign()
    }
    
    public static Vector3I translateToGridPos(Vector3 pos)
    {
        return (Vector3I) pos;
    }
    
    public static float getTickSpeed()
    {
        return 5f;
    }

    private static int i = 0;
    public static void tickPlaceables()
    {
        foreach (Cell cell in grid.Values)
        {
            if (!cell.hasCellItem()) continue;

            if (cell.getCellItem() is IPlaceable placeable)
            {
                placeable.tick();
            }
        }
        i++;
        GD.Print("Tick!!!   :"+i);
    }
    
    
    
}
