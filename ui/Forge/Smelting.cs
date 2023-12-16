using GameOff2023.globals.Inventory;
using Godot;

namespace GameOff2023.ui.Forge;

public partial class Smelting : Control, IItemHolder
{
    
    [Export]private Slot[] playerSlots;
    [Export]private Slot inputSlot;
    [Export]private Slot outputSlot;
    [Export] private GridContainer playerGridContainer;
    [Export] private ProgressBar progressBar;
    private Player player;
    private IInventory playerInventory;
    private IInventory inputInventory;
    private IInventory outputInventory;
    private Slot grabbedItem { get; set; }
    private Slot hoverOverItem { get; set; }
    private int slotIndex;
    private float smeltingTime = 2f;
    private Vector2 lastMousePos;
    private Texture2D heldItemTexture;
    [Export] private Sprite2D heldItemSprite;
    [Export] private GridContainer inputGrid;
    [Export] private GridContainer outputGrid;
    
    

    
    
    public override void _Ready()
    {
        player = player_instance.GetPlayer();
        playerInventory = player.inventory;
        inputInventory = new InventoryAPI();
        outputInventory = new InventoryAPI();
        
        
        if (playerGridContainer != null)
        {
            playerSlots = new Slot[playerGridContainer.GetChildCount()];
			
            for (int i = 0; i < playerGridContainer.GetChildCount(); i++)
            {
                Node childNode = playerGridContainer.GetChild(i);
                playerSlots[i] = childNode as Slot;
               
                GD.Print("slot loaded");
            }
        }
        outputSlot.setLabel("");
        inputSlot.setLabel("");
        UpdatePlayerSlots();
        Visible = false;
        progressBar.Visible = true;
        progressBar.MaxValue = smeltingTime;
    }
    
    public void UpdatePlayerSlots()
    {
        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].update(null);
        }
        
        foreach (var kvp in playerInventory.GetInventory())
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;

            
            Slot targetSlot = FindSlot(item, playerSlots);
            
            if (targetSlot != null)
            {
                GD.Print($"Updating player slot {itemEnum} with {item.quantity} items. itemIndex: {targetSlot.GetIndex()}");
                targetSlot.update(item);
                targetSlot.SetCurrentSlotItem(item);
            }
        }
    }

    public void UpdateOutputSlot()
    {
        outputSlot.update(null);
        
        foreach (var kvp in outputInventory.GetInventory())
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;
            
            outputSlot.update(item);
            outputSlot.SetCurrentSlotItem(item);
        }
    }
    public void UpdateInputSlot()
    {
        inputSlot.update(null);
        
        foreach (var kvp in inputInventory.GetInventory())
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;
            
            inputSlot.update(item);
            inputSlot.SetCurrentSlotItem(item);
        }
    }
    
    private bool grabbed = false;
    public override void _Process(double delta)
    {
        if (grabbedItem is not null && grabbedItem.getItem() is not null)
        {
            if (Input.IsActionJustPressed("left_mouse_click"))
            {
                SetGrabbedState(true);
                lastMousePos = GetGlobalMousePosition();
            }

            if (lastMousePos.DistanceTo(GetGlobalMousePosition()) > 2)
            {
                
                if (Input.IsActionPressed("left_mouse_click"))
                { 
                    grabbed = true;
                }
                if (Input.IsActionJustReleased("left_mouse_click"))
                {
                    grabbed = false;
                    swapItems();
                    SetGrabbedState(false);
                }
            }
        }
        if (inputSlot.getItem() != null)
        {
            CookItem(delta);
        }
        moveSprite();
    }
    
    
    private void CookItem(double delta)
    {
        progressBar.MaxValue = smeltingTime;
        progressBar.Value += delta;

        if (progressBar.Value >= smeltingTime)
        {
            CompleteCooking();
        }
    }

    private void CompleteCooking()
    {
        ItemEnum inputItemType = inputSlot.getItem().type;
        inputInventory.RemoveItem(inputItemType, 1);
        outputInventory.AddItem(inputItemType, 1);

        UpdateOutputSlot();
        UpdateInputSlot();

        progressBar.Value = 0;
    }
    
    private void CancelCooking()
    {
        progressBar.Value = 0;
    }
    public void moveSprite()
    {
        if (grabbed)
        {
            heldItemTexture = GD.Load<Texture2D>(grabbedItem.getItem().image);
            float desiredWidth = 40.0f;
            float desiredHeight = 40.0f;
            float originalWidth = heldItemTexture.GetWidth();
            float originalHeight = heldItemTexture.GetHeight();
			
            heldItemSprite.Texture = heldItemTexture;
            heldItemSprite.Scale = new Vector2(desiredWidth / originalWidth, desiredHeight / originalHeight);
            heldItemSprite.Texture = heldItemTexture;
            heldItemSprite.Position = GetGlobalMousePosition();
            heldItemSprite.Visible = true;
        }
        else
        {
            heldItemTexture = null;
            heldItemSprite.Texture = heldItemTexture;   
            heldItemSprite.Visible = false;
        }
        
    }

    public void swapItems()
    {
        int grabbedIndex = currentSlot;
        int targetIndex = slotIndex;

        GD.Print("SWAPPED");
        if (grabbedItem != null && hoverOverItem != null)
        {
            if (grabbedItem.GetParent() != hoverOverItem.GetParent())
            {
                if (grabbedItem.GetParent().Name == "PlayerGridContainer" && hoverOverItem.GetParent().Name == "Input" && inputSlot.getItem() == null)
                {
                    TransferItems(playerInventory, inputInventory, inputGrid,targetIndex);
                    UpdateInputSlot();
                    UpdatePlayerSlots();
                }else if (grabbedItem.GetParent().Name == "Output" &&
                          hoverOverItem.GetParent().Name == "PlayerGridContainer")
                {
                    TransferItems(outputInventory, playerInventory, playerGridContainer, targetIndex);
                    UpdateOutputSlot();
                    UpdatePlayerSlots();
                }else if(grabbedItem.GetParent().Name == "Input" && hoverOverItem.GetParent().Name == "PlayerGridContainer")
                {
                    TransferItems(inputInventory, playerInventory, playerGridContainer, targetIndex);
                    UpdateInputSlot();
                    UpdatePlayerSlots();
                    CancelCooking();
                }
            }else
            {
                grabbedItem.GetParent().MoveChild(grabbedItem, targetIndex);
                hoverOverItem.GetParent().MoveChild(hoverOverItem, grabbedIndex);
            }
        }
        
    }



    public void SetGrabbedItem(Slot slot)
    {
	    grabbedItem = slot;
    }

    public void SetHoveredItem(Slot slot)
    {
        hoverOverItem = slot;
    }
    
    public void SetGrabbedState(bool state)
    {
        foreach (var slot in playerSlots)
        {
            slot.SetGrabbedState(state);
        }
        inputSlot.SetGrabbedState(state);
        outputSlot.SetGrabbedState(state);
    }

	
    public void SetSlotIndex(int index)
    {
	    slotIndex = index;
    }

    private int currentSlot;
    public void SetCurrentSlotIndex(int index)
    {
        currentSlot = index;
    }
    
    public Slot GetGrabbedItem()
    {
	    return grabbedItem;
    }
    private Slot FindSlot(Item item, Slot[] slots)
    {
        foreach (var slot in slots)
        {
            if (slot.GetCurrentSlotItem() == null || slot.GetCurrentSlotItem().Equals(item))
            {
                return slot;
            }
        }
        return null;
    }
    public void RemoveItemFromSlot(Slot slot)
    {
        slot.update(null);
        grabbedItem = null;
    }

    public void TransferItems(IInventory fromInventory, IInventory toInventory, GridContainer toGrid, int targetIndex)
    {
        if (toInventory.CanAddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity))
        {
            toInventory.AddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity);
            fromInventory.RemoveItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity);
            int childCount = toGrid.GetChildCount();
            if (childCount > 0)
            {
                Control lastItem = toGrid.GetChild(childCount - 1) as Control;

                toGrid.MoveChild(lastItem, targetIndex);
            }
            RemoveItemFromSlot(grabbedItem);
        }
    }
}