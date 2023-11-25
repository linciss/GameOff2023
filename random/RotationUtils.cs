using Godot;

namespace GameOff2023.random;

public class RotationUtils
{
    public static bool isReverseYRotation(float rotation1, float rotation2)
    {
        GD.PrintErr("reverse Rota");
        GD.Print("Rotation 1: " + rotation1);
        GD.Print("Rotation 2: " + rotation2);
        return (rotation1 + rotation2) % 360 == 0;
    }
}