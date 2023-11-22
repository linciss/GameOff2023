using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


    public enum ItemEnum
    {
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
        public int quantity { get; set; }
        public string image { get; set; }
        
        public List<Recipe> recipe { get; set; }
        
        
        public int maxQuantity = 200;

        public Item(string name, int quantity, string image)
        {
            this.name = name;
            this.quantity = quantity;
            this.image = image;
        }

    public void AddQuantity(int amount)
    {
        if (quantity + amount > maxQuantity)
        {
            quantity = maxQuantity;
        }
        else
        {
            quantity += amount;
        }
    }

    public void RemoveQuantity(int amount)
    {
   
         quantity -= amount;
        
    }
    
    public Item Clone()
    {
        return new Item(name, quantity, image);
    }

}


