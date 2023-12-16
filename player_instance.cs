using Godot;
using System;

public partial class player_instance : Node
{
    private static Player player;
    public static Player GetPlayer()
    {
        return player;
    }

    public static void setPlayer(Player playerInstance)
    {
        player = playerInstance;
    }
}
