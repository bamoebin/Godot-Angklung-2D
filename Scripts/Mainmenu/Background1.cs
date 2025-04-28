using Godot;
using System;

public partial class Background1 : Sprite2D
{
	public override void _Process(double delta)
	{
		Position += (GetGlobalMousePosition() * (float)delta) - Position;
	}
}
