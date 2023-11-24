namespace GameOff2023.entities.placeable;

public interface IMachineOutput
{
    public bool canOutput(Item item, ICellItem cellItem);
    
    public void output(Item item);
}