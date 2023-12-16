using Godot;

namespace GameOff2023.random;

public class RotationUtils
{
    public static bool isReverseYRotation(float rotation1, float rotation2)
    {
        GD.PrintErr("reverse Rota");
        GD.Print("Rotation 1: " + rotation1);
        GD.Print("Rotation 2: " + rotation2);
        // Normalize angles to the range [-180, 180]
        rotation1 %= 360f;
        rotation2 %= 360f;

        if (rotation1 > 180f)
        {
            rotation1 -= 360f;
        }

        if (rotation2 > 180f)
        {
            rotation2 -= 360f;
        }

        // Calculate absolute difference between angles
        float angleDiff = Mathf.Abs(rotation1 - rotation2);

        // Determine if angles are pointing in opposite directions
        bool isOpposite = angleDiff > 90f && angleDiff < 270f;

        return isOpposite;

    }
}