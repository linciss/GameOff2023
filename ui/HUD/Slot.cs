using Godot;
using System;
using GameOff2023.globals.Inventory;

public partial class Slot : Panel, IItemHolder
{
	private bool grabbed = false;
	[Export]
	private Sprite2D sprite;
	[Export]
	private Godot.Label label;

	private Item item;
	private Slot hoveredItem { get; set; }
	private int slotIndex;
	private int currentSlotIndex;
	private Item currentSlotItem;
	private Slot grabbedItem;
	
	public void update(Item item)
	{
		if (item == null)
		{
			this.item = null;
			label.Text = null;
			sprite.Texture = null;
			sprite.Visible = false;
		}
		else
		{
			this.item = item;
			Texture2D texture = GD.Load<Texture2D>(item.image);
			
			if (label != null)
			{
				label.Text = item.quantity.ToString();
				label.Visible = true;
			}

			float desiredWidth = 17.0f;
			float desiredHeight = 17.0f;
			float originalWidth = texture.GetWidth();
			float originalHeight = texture.GetHeight();

			sprite.Texture = texture;
			sprite.Scale = new Vector2(desiredWidth / originalWidth, desiredHeight / originalHeight);
			sprite.Visible = true;
		}
	}

	public void _on_mouse_entered()
	{
		if (GetParent().GetParent().GetParent() is IItemHolder itemHolder)
		{
			if (!grabbed)
			{
				itemHolder.SetGrabbedItem(this);
				itemHolder.SetCurrentSlotIndex(GetIndex());
			}
			else
			{
				itemHolder.SetHoveredItem(this);;
				itemHolder.SetSlotIndex(GetIndex()); 
			}
		}
	} 
	public void SetGrabbedItem(Slot slot)
	{
		grabbedItem = slot;
	}

	public void SetHoveredItem(Slot slot)
	{
		hoveredItem = slot;
	}

	public void SetGrabbedState(bool state)
	{
		grabbed = state;
	}
	
	public void SetSlotIndex(int index)
	{
		slotIndex = index;
	}

	public void SetCurrentSlotItem(Item item)
	{
		currentSlotItem = item;
	}

	public Item GetCurrentSlotItem()
	{
		return currentSlotItem;
	}
	public void SetCurrentSlotIndex(int index)
	{
		currentSlotIndex = index;
	}

	public Item getItem()
	{
		return item;
	}
	
	public void AddItemToSlot(Slot slot)
	{
		if (slot != null)
		{
			update(slot.getItem());
		}
	}

	public Slot GetGrabbedItem()
	{
		return grabbedItem;
	}

	public void RemoveItemFromSlot(Slot slot)
	{
		update(null);
	}
}
