using Godot;
using System;
namespace Godot;

public partial class SliderHelper : Control
{
	[Export] public BaseKarya TargetKarya;

	// Referensi Slider Control
	private HSlider rotationSpeedSlider;
	private HSlider floatingSpeedSlider;
	
	// Referensi Label Control
	private Label rotationSpeedLabel;
	private Label floatingSpeedLabel;

	public override void _Ready()
	{
	
		rotationSpeedSlider = GetNodeOrNull<HSlider>("SliderRotationSpeed");
		floatingSpeedSlider = GetNodeOrNull<HSlider>("SliderFloatingSpeed");

		
		if (rotationSpeedSlider != null)
		{
			rotationSpeedSlider.ValueChanged += OnRotationSpeedChanged;
			
		
			if (TargetKarya is Karya4 karya4)
			{
				rotationSpeedSlider.Value = karya4.rotationSpeed;
				UpdateRotationSpeedLabel(karya4.rotationSpeed);
			}
		}
		else
		{
			GD.PrintErr("SliderRotationSpeed tidak ditemukan!");
		}
		
		if (floatingSpeedSlider != null)
		{
			floatingSpeedSlider.ValueChanged += OnFloatingSpeedChanged;
			
			if (TargetKarya is Karya4 karya4)
			{
				floatingSpeedSlider.Value = karya4.floatingSpeed;
				UpdateFloatingSpeedLabel(karya4.floatingSpeed);
			}
		}
		else
		{
			GD.PrintErr("SliderFloatingSpeed tidak ditemukan!");
		}
	}

	private void OnRotationSpeedChanged(double value)
	{
		if (TargetKarya is Karya4 karya4)
		{
			karya4.rotationSpeed = (float)value;
			UpdateRotationSpeedLabel(value);
		}
	}
	
	private void OnFloatingSpeedChanged(double value)
	{
		if (TargetKarya is Karya4 karya4)
		{
			karya4.floatingSpeed = (float)value;
			UpdateFloatingSpeedLabel(value);
		}
	}
	
	private void UpdateRotationSpeedLabel(double value)
	{
		if (rotationSpeedLabel != null)
			rotationSpeedLabel.Text = $"Rotation Speed: {value:F1}";
	}
	
	private void UpdateFloatingSpeedLabel(double value)
	{
		if (floatingSpeedLabel != null)
			floatingSpeedLabel.Text = $"Floating Speed: {value:F1}";
	}
	
}
