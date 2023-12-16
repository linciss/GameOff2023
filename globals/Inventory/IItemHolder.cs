namespace GameOff2023.globals.Inventory;

public interface IItemHolder
{
    void SetHoveredItem(Slot slot);
    void SetGrabbedItem(Slot slot);
    void SetGrabbedState(bool state);
    void SetSlotIndex(int index);
    void SetCurrentSlotIndex(int index);
    
}