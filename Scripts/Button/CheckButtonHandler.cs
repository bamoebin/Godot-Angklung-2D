using Godot;
namespace Godot;
public partial class CheckButtonHandler : Control
{
	[Export] public BaseKarya TargetKarya; // Hubungkan di Inspector 
	//Untuk dokumentasi, langsung saja pilih Node2D yang tersedia di setiap scene

	public override void _Ready()
{
	var checkAxis = GetNodeOrNull<CheckButton>("CheckButtonAxis");
	var checkMarginBox = GetNodeOrNull<CheckButton>("CheckButtonMarginBox");
	var checkSimpleAnimation = GetNodeOrNull<CheckButton>("CB_SimpleAnim");
	var checkFlowerAnimation = GetNodeOrNull<CheckButton>("CB_FlowerAnim");

	if (checkSimpleAnimation != null)
		checkSimpleAnimation.Toggled += OnSimpleAnimationToggled;
	else
		GD.PrintErr("CB_SimpleAnim tidak ditemukan!");

	if (checkAxis != null)
		checkAxis.Toggled += OnAxisToggled;
	else
		GD.PrintErr("CheckButtonAxis tidak ditemukan!");

	if (checkMarginBox != null)
		checkMarginBox.Toggled += OnMarginBoxToggled;
	else
		GD.PrintErr("CheckButtonMarginBox tidak ditemukan!");

	if (checkFlowerAnimation != null)
		checkFlowerAnimation.Toggled += OnFlowerAnimationToggled;
	else
		GD.PrintErr("CB_FlowerAnim tidak ditemukan!");
}
	
	private void OnSimpleAnimationToggled(bool buttonPressed)
	{
		if (TargetKarya != null)
		{
			TargetKarya.isSimpleAnimation = buttonPressed;
			TargetKarya.QueueRedraw();
		}
	}

	private void OnAxisToggled(bool buttonPressed)
	{
		if (TargetKarya != null)
		{
			TargetKarya.isDrawAxis = buttonPressed;
			TargetKarya.QueueRedraw(); 
		}
	}

	private void OnMarginBoxToggled(bool buttonPressed)
	{
		if (TargetKarya != null)
		{
			TargetKarya.isDrawMarginBox = buttonPressed;
			TargetKarya.QueueRedraw(); 
		}
	}

	private void OnFlowerAnimationToggled(bool buttonPressed)
{
	if (TargetKarya != null )
	{
		TargetKarya.isFlowerAnimationActive = buttonPressed;
		TargetKarya.QueueRedraw();
	}
}
}
