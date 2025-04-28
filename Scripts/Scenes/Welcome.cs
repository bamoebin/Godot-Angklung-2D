using Godot;
using System.Collections.Generic;

public partial class Welcome : Control
{
	private Dictionary<string, string> scenePaths = new Dictionary<string, string>
	{
		{ "BtnKaryas", "res://Scenes/Karyas.tscn" },
		{ "BtnAbout", "res://Scenes/About.tscn" },
		{ "BtnGuide", "res://Scenes/Guide.tscn" }
	};

	public override void _Ready()
	{

		foreach (var buttonName in scenePaths.Keys)
		{
			TextureButton button = GetNodeOrNull<TextureButton>("Panel/" + buttonName);
			
			if (button == null)
			{
				GD.PrintErr($"ERROR: {buttonName} tidak ditemukan di dalam scene!");
			}
			else
			{
				//GD.Print($"{buttonName} ditemukan! Menghubungkan event...");
				button.Pressed += () => OnButtonPressed(buttonName);
			}
		} 
		GetNodeOrNull<TextureButton>("Panel/BtnExit").Pressed += OnExitPressed;
		////Algoritma foreach di bawah, kalo ada yang Null atau ada di Dictionary, tetapi di Scene belum ada, jadi error
		//foreach (var buttonName in scenePaths.Keys)
		//{
			//TextureButton button = GetNodeOrNull<TextureButton>("Panel/" + buttonName);
			//button.Pressed += () => OnButtonPressed(buttonName);
		//}		
		
	}

	private void OnButtonPressed(string buttonName)
	{
		if (scenePaths.TryGetValue(buttonName, out string scenePath))
		{
			ChangeScene(scenePath);
		}
	}

	public void OnExitPressed()
	{
		GD.Print("Tombol Exit ditekan!");
		GetTree().Quit();
	}

	private void ChangeScene(string scenePath)
	{
		PackedScene scene = GD.Load<PackedScene>(scenePath);
		if (scene != null)
		{
			GetTree().ChangeSceneToPacked(scene);
		}
		else
		{
			GD.PrintErr("Scene Tidak Ada: " + scenePath);
		}
	}
}
