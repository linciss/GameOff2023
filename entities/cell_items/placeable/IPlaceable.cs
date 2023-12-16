using GameOff2023.entities.placeable;

using Godot;

namespace GameOff2023.entities;

public interface IPlaceable
{
    public PlaceableType getType();
    
    public Vector3 getRotation();
    
    public InventoryAPI getInventory();

    public void tick();
}