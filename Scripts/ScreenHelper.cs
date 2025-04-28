using Godot;
using System;

public partial class ScreenHelper
{
	public static int ScreenWidth => DisplayServer.WindowGetSize().X;
	public static int ScreenHeight => DisplayServer.WindowGetSize().Y;

	public const int MarginLeft = 50;
	public const int MarginTop = 50;

	public static int MarginRight => ScreenWidth - MarginLeft;
	public static int MarginBottom => ScreenHeight - MarginTop;
}
