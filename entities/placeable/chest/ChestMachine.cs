using Godot;

namespace GameOff2023.entities.placeable.test;

public partial class ChestMachine : PlaceableCell
{
    private InventoryAPI inventory;
    
    public ChestMachine(SceneTree tree) : base("ChestMachine", null, PlaceableType.Chest, tree)
    {
        node = this;
        
        inventory = new InventoryAPI();
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/placeable/chest/chest_machine.tscn");
        
        AddChild(prefab.Instantiate());

        tree.CurrentScene.AddChild(this);
    }
    
    

}