using Godot;
namespace Godot;
public partial class SceneButton : BaseButton
{
	[Export(PropertyHint.File, "*.tscn")] //buat Nambahin properti di Inspector, jangan lupa build ulang projectnya ke .Net
	public string ScenePath = "";
	
	public override void _Ready()
	{
		if (string.IsNullOrEmpty(ScenePath))
		{
			ScenePath = GetDefaultScenePath();
		}
	}
	
	public override void _Pressed()
	{
		ChangeScene(ScenePath);
	}
	
	protected virtual string GetDefaultScenePath()
	{
		return "res://Scenes/Welcome.tscn"; // Default scene jika tidak diatur di Inspector
	}
	
	protected virtual void ChangeScene(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			Error result = GetTree().ChangeSceneToFile(path);
			if (result != Error.Ok)
			{
				GD.PrintErr("Scene Tidak Ada: " + path);
			}
		}
	}
}
