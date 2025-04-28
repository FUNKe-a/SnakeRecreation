using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Apple : Area2D
{
    public async Task<bool> IsAppleOnSnake()
    {
        await ToSignal(GetTree(), "physics_frame");
        return HasOverlappingAreas();
    }
}
