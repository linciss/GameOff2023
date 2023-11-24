using Godot;

namespace GameOff2023.entities.placeable.test;

public partial class BeltMachine : PlaceableCell
{
    private InventoryAPI inventory;
    
    public BeltMachine(SceneTree tree) : base("BeltMachine", null, PlaceableType.Belt, tree)
    {
        node = this;
        
        inventory = new InventoryAPI();
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/placeable/belt/belt_machine.tscn");
        
        AddChild(prefab.Instantiate());

        tree.CurrentScene.AddChild(this);
    }
    
    
    
    

}