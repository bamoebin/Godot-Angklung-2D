using Godot;
using System;

public partial class Background2 : Node2D
{
	public override void _Process(double delta)
	{
		Position += (GetGlobalMousePosition() * (float)delta)/4 - Position;
	}
}
