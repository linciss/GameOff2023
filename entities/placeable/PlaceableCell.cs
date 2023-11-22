using GameOff2023.entities.placeable;
using Godot;

namespace GameOff2023.entities.placeable;
public partial class PlaceableCell : Cell
{
    [Export]
    PlaceableType type;

    public override void _Ready()
    {
        name = Name;
        node = this;
        
        GridSystem.setPosition(
            Position, 
            this
        );
    }
    
    public PlaceableCell(string name, Node3D node, PlaceableType type) : base(name, node, ItemEnum.None)
    {
        this.type = type;
    }

    public PlaceableType getType()
    {
        return type;
    }
    

}