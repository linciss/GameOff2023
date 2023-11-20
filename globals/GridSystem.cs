using Godot;


public partial class GridSystem : Node
{
    [Export]
    private Vector3 cellSize = new Vector3(1, 1, 1);

    static Godot.Collections.Dictionary<Node3D, Vector3I> grid = new Godot.Collections.Dictionary<Node3D, Vector3I>();

    public static void setPosition(Vector3 newPos, Node3D node)
    {
        node.Position = newPos.Floor() + new Vector3(1, 1, 1) * 0.5f;
        if (grid.ContainsKey(node))
        {
            grid[node] = (Vector3I)newPos;
        }
        else
        {
            grid.Add(node, (Vector3I)newPos);
        }
    }

    public static Godot.Collections.Dictionary<Node3D, Vector3I> getGrid()
    {
        return grid;
    }

    public static Vector3I getGridPosition(Node3D node)
    {
        return grid[node];
    }

    public static Vector3I getGridPosition(Vector3 pos)
    {
        return (Vector3I) pos;
    }

    public static Node3D getNodeAt(Vector3I pos)
    {
        foreach (Node3D node in grid.Keys)
        {
            if (grid[node] == pos)
            {
                return node;
            }
        }

        return null;
    }
}
