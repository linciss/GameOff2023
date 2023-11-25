namespace GameOff2023.entities.placeable;

public interface IMachineInput
{
    public bool canInput(ItemEnum item, int quantity, ICellItem cellItem);
    
    public void input(ItemEnum item, int quantity);
}