using System;
using System.Collections.Generic;
using Godot;

public interface IInventory 
{
    Dictionary<ItemEnum, Item> GetInventory();
    event Action InventoryChanged;
    void AddItem(ItemEnum itemEnum, int quantity);
    void RemoveItem(ItemEnum itemEnum, int quantity);
    bool CanAddItem(ItemEnum itemEnum, int quantity);
    void PrintAllItems();
}