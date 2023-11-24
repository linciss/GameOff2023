namespace GameOff2023.entities.placeable;

public interface IMachineOutput
{
    public bool canOutput(Item item);
    
    public void output(Item item);
}