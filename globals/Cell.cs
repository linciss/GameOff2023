using Godot;
using System;

public partial class Cell : Node
{

    public string name;

    public Node3D node;

    public ItemEnum item;

    public Cell(string name, Node3D node, ItemEnum item)
    {
        this.name = name;
        this.node = node;
        this.item = item;
    }


    override public string ToString()
    {
        return name + " " + node + " " + item;
    }
}
