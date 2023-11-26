namespace GameOff2023.globals.Inventory;

public interface IItemTransfer
{
    bool CanTransferItem(ItemEnum itemType, int quantity);
    bool TransferItem(IInventory targetInventory, ItemEnum itemType, int quantity);
    void UpdateSlots();
}