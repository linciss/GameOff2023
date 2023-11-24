using System;
using System.Collections.Generic;
using Godot;

public interface IInventory 
{
    Dictionary<ItemEnum, Item> GetInventory();
    event Action InventoryChanged;
}