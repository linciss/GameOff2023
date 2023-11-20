using System;
using System.ComponentModel.DataAnnotations;


    public enum ItemEnum
    {
        RawSteel,
        SteelIngot,
        RawCopper,
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
        public  int maxQuantity = 200;

        public Item(string name, int quantity)
        {
            this.name = name;
            this.quantity = quantity;
        }
   }


