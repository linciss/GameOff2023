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
    public string name { get; set; }
    public ItemEnum type { get; set; }
    public int quantity { get; set; }
    public string image { get; set; }
    public List<Recipe> recipe { get; set; }
    public int maxQuantity { get; set; }

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
        GD.PrintErr("TRYING TO ADD QUANITTY");
        GD.Print("nax quantity: " + maxQuantity);
        if (quantity + amount > maxQuantity)
        {
            quantity = maxQuantity;
            GD.Print("max quantity reached");
            return false;
        }
        quantity += amount;
        GD.Print("amount to be addeed: " + amount);
        GD.Print("new quantity: " + quantity);
        return true;
    }

    /**
     * false if quantity is 0
     */
    public bool RemoveQuantity(int amount)
    {
        if (quantity - amount <= 0)
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


