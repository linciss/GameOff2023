using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public enum ItemEnum
{
    None,
    RawSteel,
    SteelIngot,
    RawCopper,
    CopperIngot,
    SteelPlate,
    CopperPlate,
    CircuitBoard,
    Forge,
    CraftingTable,
    Hydrogen
}

public class Item
{
    public string name;

    public ItemEnum type;
    public int quantity;
    public string image;

    public List<Recipe> recipe;
    
    public int maxQuantity;

    public Item(string name, int quantity, int maxQuantity, string image)
    {
        this.name = name;
        this.type = (ItemEnum) Enum.Parse(typeof(ItemEnum), name);;
        this.quantity = quantity;
        this.maxQuantity = maxQuantity;
        this.image = image;
    }

    public bool AddQuantity(int amount)
    {
        if (quantity + amount > maxQuantity)
        {
            quantity = maxQuantity;
            return false;
        }
        quantity += amount;
        return true;
    }

    public bool RemoveQuantity(int amount)
    {
        if (quantity - amount < 0)
        {
            return false;
        }
        quantity -= amount;
        return true;
    }
    
    public ItemEnum GetType()
    {
        return type;
    }
    
    public Item Clone()
    {
        return new Item(name, quantity, maxQuantity, image);
    }

}


