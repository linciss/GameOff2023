using GameOff2023.entities.placeable;

using Godot;

namespace GameOff2023.entities;

public interface IPlaceable
{
    public PlaceableType getType();

    public void tick();
}