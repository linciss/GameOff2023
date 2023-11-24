namespace GameOff2023.entities.placeable;

public interface IMachineInput
{
    public bool canInput(Item item, ICellItem cellItem);
    
    public void input(Item item);
}