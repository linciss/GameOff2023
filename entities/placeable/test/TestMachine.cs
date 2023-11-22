using Godot;

namespace GameOff2023.entities.placeable.test;

public partial class TestMachine : PlaceableCell
{
    public TestMachine(SceneTree tree) : base("TestMachine", null, PlaceableType.TestMachine, tree)
    {
        node = this;
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/placeable/test/test_machine.tscn");
        
        AddChild(prefab.Instantiate());

        tree.CurrentScene.AddChild(this);
    }

}