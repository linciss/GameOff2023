using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

public partial class ItemFactory : Node
    {

        public static Dictionary<ItemEnum, Item> allItems = new Dictionary<ItemEnum, Item>();

        static ItemFactory()
        {
            InitializeItems();
        }

        public static void InitializeItems()
        {
            string jsonFilePath = "res://entities/player/Inventory/ItemJson.json";
 
            string jsonContent = FileAccess.GetFileAsString(jsonFilePath);

            List<Item> items = JsonSerializer.Deserialize<List<Item>>(jsonContent);

            foreach (var item in items)
            {
                if (Enum.TryParse<ItemEnum>(item.name, out ItemEnum itemEnum))
                {
                    allItems[itemEnum] = item;
                }
                else
                {
                    Console.WriteLine($"Bitch not working {item.name}");
                }
            }
        }
        public static void PrintAllItems()
        {
            foreach (var kvp in allItems)
            {
                GD.Print($"Name: {kvp.Value.name}, Quantity: {kvp.Value.quantity}");
            }
        }

    }


