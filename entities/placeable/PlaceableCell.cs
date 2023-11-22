using GameOff2023.entities.placeable;
using Godot;

namespace GameOff2023.entities.placeable;
public partial class PlaceableCell : Cell
{
    [Export]
    PlaceableType type;
    
    SceneTree tree;

    public PlaceableCell(string name, Node3D node, PlaceableType type, SceneTree tree) : base(name, node, ItemEnum.None)
    {
        this.type = type;
        this.tree = tree;
    }

    public PlaceableType getType()
    {
        return type;
    }
    

}