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
    Item GetItemAtIndex(int index);
    int CalculateTransferQuantity(ItemEnum itemEnum, int requestedQuantity);
    void PrintAllItems();
}